#! /bin/bash
DEV=../stick
mkdir -p $DEV/$1
cp -R $2 $DEV/$1/
chmod -R 777 $DEV/$1/$2
