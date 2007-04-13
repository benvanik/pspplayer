#! /bin/bash
psp-objdump -d --adjust-vma=0x08900000 $1 > $1.dis.txt
