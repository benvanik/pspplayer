#! /bin/bash

DEV=../stick
mkdir -p $DEV/$1
echo src: $1/$2/ target: $DEV/$1/$2
find $2/ -not -regex .*\.svn.* -not -wholename $2/ -print -exec cp '{}' $DEV/$1/$2/ \;
chmod -R 777 $DEV/$1/$2

