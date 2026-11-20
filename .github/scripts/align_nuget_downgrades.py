#!/usr/bin/env python3
"""Raise direct PackageReference versions reported as NU1605 downgrades.

Dependabot can bump one project's pin while other projects keep the older
direct reference. Restore then fails with NU1605 because warnings are errors,
so the lock-file job never reaches its commit step. This script reads that
restore log and raises each named direct reference to the higher version.
"""

from __future__ import annotations

import re
import sys
from pathlib import Path

NU1605_LINE = re.compile(
    r"^(?P<project>.+?\.csproj)\s*:\s*(?:warning|error) NU1605:"
    r".*Detected package downgrade: (?P<package>\S+) from (?P<higher>\S+) to (?P<lower>\S+)",
    re.IGNORECASE,
)
PACKAGE_REFERENCE = re.compile(r"<PackageReference\b(?P<attrs>[^>]*?)(?P<close>\s*/>|>)", re.IGNORECASE)
INCLUDE_ATTRIBUTE = re.compile(r"""\bInclude\s*=\s*(?P<quote>["'])(?P<value>.*?)(?P=quote)""", re.IGNORECASE)
VERSION_ATTRIBUTE = re.compile(r"""\bVersion\s*=\s*(?P<quote>["'])(?P<value>.*?)(?P=quote)""", re.IGNORECASE)


def clean_version(value: str) -> str:
    return value.rstrip(".,);")


def parse_downgrades(log: str) -> list[tuple[str, str, str, str]]:
    """Return unique (project, package, lower, higher) tuples in log order."""
    found: list[tuple[str, str, str, str]] = []
    seen: set[tuple[str, str, str, str]] = set()
    for line in log.splitlines():
        match = NU1605_LINE.search(line.strip())
        if match is None:
            continue
        item = (
            match.group("project"),
            match.group("package"),
            clean_version(match.group("lower")),
            clean_version(match.group("higher")),
        )
        if item not in seen:
            seen.add(item)
            found.append(item)
    return found


def bump_package_reference(text: str, package: str, lower: str, higher: str) -> tuple[str, int]:
    """Raise Version attributes on matching PackageReference elements."""
    count = 0

    def replace(match: re.Match[str]) -> str:
        nonlocal count
        attrs = match.group("attrs")
        include = INCLUDE_ATTRIBUTE.search(attrs)
        version = VERSION_ATTRIBUTE.search(attrs)
        if include is None or version is None:
            return match.group(0)
        if include.group("value").lower() != package.lower():
            return match.group(0)
        if version.group("value") != lower:
            return match.group(0)
        count += 1
        updated_attrs = (
            attrs[: version.start("value")] + higher + attrs[version.end("value") :]
        )
        return f"<PackageReference{updated_attrs}{match.group('close')}"

    return PACKAGE_REFERENCE.sub(replace, text), count


def align_downgrades(log: str, repo_root: Path) -> list[str]:
    """Apply every reported downgrade. Return a summary line per edit.

    Exits the process when the log has no NU1605 entries or a reported project
    does not contain the lower direct reference.
    """
    downgrades = parse_downgrades(log)
    if not downgrades:
        print("Restore failed without an NU1605 downgrade to align.", file=sys.stderr)
        raise SystemExit(1)

    repo_root = repo_root.resolve()
    summaries: list[str] = []
    for project, package, lower, higher in downgrades:
        path = Path(project)
        if not path.is_file():
            candidate = repo_root / project
            path = candidate if candidate.is_file() else path
        resolved = path.resolve()
        if not resolved.is_file() or not resolved.is_relative_to(repo_root):
            print(f"NU1605 project file is outside the repository: {project}", file=sys.stderr)
            raise SystemExit(1)

        original = path.read_text(encoding="utf-8")
        updated, count = bump_package_reference(original, package, lower, higher)
        if count == 0:
            print(
                f"No PackageReference {package} {lower} in {path}.",
                file=sys.stderr,
            )
            raise SystemExit(1)
        if updated != original:
            path.write_text(updated, encoding="utf-8")
        summaries.append(f"Raised {package} from {lower} to {higher} in {path} ({count})")
    return summaries


def main(argv: list[str]) -> int:
    if len(argv) != 2:
        print(f"Usage: {argv[0]} restore.log", file=sys.stderr)
        return 2
    log_path = Path(argv[1])
    log = log_path.read_text(encoding="utf-8")
    for summary in align_downgrades(log, Path.cwd()):
        print(summary)
    return 0


if __name__ == "__main__":
    raise SystemExit(main(sys.argv))
