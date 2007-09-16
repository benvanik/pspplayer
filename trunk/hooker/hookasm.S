
	.set noreorder
	.set noat
	
	# hooking.c
	.global _OnHookHit
	.global _OnHookReturn
	
	.global SetK1
	.ent    SetK1
SetK1:
	move $v0, $k1
	jr	 $ra
	move $k1, $a0
	.end SetK1

	.global _HookEntry
	.ent    _HookEntry
_HookEntry:
	addiu	$sp, $sp, -(4*8) # Allocate space for 6 args + $ra + hooknum
	sw		$ra, 0($sp)		 # Save $ra and args
	sw		$a0, 4($sp)
	sw		$a1, 8($sp)
	sw		$a2, 12($sp)
	sw		$a3, 16($sp)
	sw		$t0, 20($sp)
	sw		$t1, 24($sp)
	sw		$v0, 28($sp)
	move	$a0, $v0		# Copy api hook number into first arg
	jal		_OnHookHit
	addiu	$a1, $sp, 4		# Copy pointer to args into second arg

	lw		$a0, 4($sp)		# Restore args
	lw		$a1, 8($sp)
	lw		$a2, 12($sp)
	lw		$a3, 16($sp)

	beq		$v0, $0, 1f
	lw		$t0, 20($sp)

	jalr	$v0				# Will call with a 32 byte overhead
	lw		$t1, 24($sp)

	sw		$v0, 4($sp)		# Save return values
	sw		$v1, 8($sp)
	lw		$a0, 28($sp)	# Load hook address

	jal		_OnHookReturn	# Print return values
	addiu	$a1, $sp, 4		# Copy pointer to return values into second arg

	lw		$v0, 4($sp)		# Restore return values
	lw		$v1, 4($sp)

1:
	lw		$ra, 0($sp)		# Restore $ra
	jr		$ra
	addiu	$sp, $sp, (4*8)
	.end    _HookEntry
	