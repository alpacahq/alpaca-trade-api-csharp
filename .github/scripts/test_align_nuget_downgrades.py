#!/usr/bin/env python3
"""Tests for the NU1605 PackageReference aligner."""

from __future__ import annotations

import tempfile
import unittest
from pathlib import Path

from align_nuget_downgrades import (
    align_downgrades,
    bump_package_reference,
    parse_downgrades,
)

CI_LOG = """
/home/runner/work/alpaca-trade-api-csharp/alpaca-trade-api-csharp/Alpaca.Markets.Extensions.Tests/Alpaca.Markets.Extensions.Tests.csproj : error NU1605: Warning As Error: Detected package downgrade: System.IO.Pipelines from 10.0.15 to 10.0.14. Reference the package directly from the project to select a different version.  [/home/runner/work/alpaca-trade-api-csharp/alpaca-trade-api-csharp/Alpaca.Markets.sln]
/home/runner/work/alpaca-trade-api-csharp/alpaca-trade-api-csharp/Alpaca.Markets.Extensions.Tests/Alpaca.Markets.Extensions.Tests.csproj : error NU1605:  Alpaca.Markets.Extensions.Tests -> Alpaca.Markets -> System.IO.Pipelines (>= 10.0.15)  [/home/runner/work/alpaca-trade-api-csharp/alpaca-trade-api-csharp/Alpaca.Markets.sln]
/home/runner/work/alpaca-trade-api-csharp/alpaca-trade-api-csharp/Alpaca.Markets.Tests/Alpaca.Markets.Tests.csproj : error NU1605: Warning As Error: Detected package downgrade: System.IO.Pipelines from 10.0.15 to 10.0.14. Reference the package directly from the project to select a different version.  [/home/runner/work/alpaca-trade-api-csharp/alpaca-trade-api-csharp/Alpaca.Markets.sln]
/home/runner/work/alpaca-trade-api-csharp/alpaca-trade-api-csharp/UsageExamples/UsageExamples.csproj : error NU1605: Warning As Error: Detected package downgrade: System.IO.Pipelines from 10.0.15 to 10.0.14. Reference the package directly from the project to select a different version.
"""

MARKETS = """
    <PackageReference Include="System.IO.Pipelines" Version="10.0.14" PrivateAssets="compile;contentfiles;build;analyzers" />
"""

TESTS = """
    <PackageReference Include="System.Threading.Channels" Version="10.0.14" />
    <PackageReference Include="System.IO.Pipelines" Version="10.0.14" />
    <PackageReference Version="10.0.14" Include="System.IO.Pipelines" />
    <PackageReference Include="System.IO.Pipelines" Version="10.0.11" />
    <PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
"""


class ParseDowngradesTests(unittest.TestCase):
    def test_ci_log_keeps_one_entry_per_project(self) -> None:
        downgrades = parse_downgrades(CI_LOG)
        self.assertEqual(
            [Path(project).name for project, _, _, _ in downgrades],
            [
                "Alpaca.Markets.Extensions.Tests.csproj",
                "Alpaca.Markets.Tests.csproj",
                "UsageExamples.csproj",
            ],
        )
        self.assertTrue(all(item[1:] == ("System.IO.Pipelines", "10.0.14", "10.0.15") for item in downgrades))

    def test_ignores_graph_detail_lines(self) -> None:
        self.assertEqual(parse_downgrades("error NU1605: Alpaca.Markets.Tests -> System.IO.Pipelines (>= 10.0.14)"), [])


class BumpPackageReferenceTests(unittest.TestCase):
    def test_raises_only_the_downgraded_version(self) -> None:
        updated, count = bump_package_reference(TESTS, "System.IO.Pipelines", "10.0.14", "10.0.15")
        self.assertEqual(count, 2)
        self.assertIn('Include="System.IO.Pipelines" Version="10.0.15"', updated)
        self.assertIn('Version="10.0.15" Include="System.IO.Pipelines"', updated)
        self.assertIn('Include="System.IO.Pipelines" Version="10.0.11"', updated)
        self.assertIn('Include="System.Threading.Channels" Version="10.0.14"', updated)
        self.assertIn('Include="Newtonsoft.Json" Version="13.0.3"', updated)

    def test_preserves_private_assets(self) -> None:
        updated, count = bump_package_reference(MARKETS, "System.IO.Pipelines", "10.0.14", "10.0.15")
        self.assertEqual(count, 1)
        self.assertIn(
            'Include="System.IO.Pipelines" Version="10.0.15" PrivateAssets="compile;contentfiles;build;analyzers"',
            updated,
        )


class AlignDowngradesTests(unittest.TestCase):
    def test_writes_matching_projects_inside_the_repository(self) -> None:
        with tempfile.TemporaryDirectory() as temp:
            root = Path(temp)
            tests = root / "Alpaca.Markets.Tests"
            tests.mkdir()
            project = tests / "Alpaca.Markets.Tests.csproj"
            project.write_text(
                '<PackageReference Include="System.IO.Pipelines" Version="10.0.14" />\n',
                encoding="utf-8",
            )
            log = (
                f"{project} : error NU1605: Warning As Error: Detected package downgrade: "
                "System.IO.Pipelines from 10.0.15 to 10.0.14. Reference the package directly.\n"
            )
            summaries = align_downgrades(log, root)
            self.assertEqual(len(summaries), 1)
            self.assertIn('Version="10.0.15"', project.read_text(encoding="utf-8"))

    def test_missing_reference_fails(self) -> None:
        with tempfile.TemporaryDirectory() as temp:
            root = Path(temp)
            project = root / "App.csproj"
            project.write_text("<Project />\n", encoding="utf-8")
            log = (
                f"{project} : error NU1605: Detected package downgrade: "
                "System.IO.Pipelines from 10.0.15 to 10.0.14.\n"
            )
            with self.assertRaises(SystemExit) as raised:
                align_downgrades(log, root)
            self.assertEqual(raised.exception.code, 1)

    def test_restore_failure_without_downgrade_fails(self) -> None:
        with self.assertRaises(SystemExit) as raised:
            align_downgrades("error NU1004: The packages lock file is inconsistent.\n", Path("."))
        self.assertEqual(raised.exception.code, 1)


if __name__ == "__main__":
    unittest.main()
