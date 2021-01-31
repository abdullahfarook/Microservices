#!/bin/bash
set -e

branch="master"

git checkout $branch
{ # try

git merge origin/develop
git push origin $branch

} || { # catch
echo "Error"
}


git checkout develop
exit 0