@echo off
git add Documentation/_site/** -f
git commit -m "Update content of .NET SDK documentation on GitHub Pages web site."
git subtree push --prefix Documentation/_site origin gh-pages