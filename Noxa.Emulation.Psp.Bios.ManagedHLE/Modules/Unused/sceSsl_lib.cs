// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

using Noxa.Utilities;
using Noxa.Emulation.Psp;
using Noxa.Emulation.Psp.Bios;
using Noxa.Emulation.Psp.Cpu;

namespace Noxa.Emulation.Psp.Bios.ManagedHLE.Modules
{
	class sceSsl_lib : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceSsl_lib";
			}
		}

		#endregion

		#region State Management

		public sceSsl_lib( Kernel kernel )
			: base( kernel )
		{
		}

		public override void Start()
		{
		}

		public override void Stop()
		{
		}

		#endregion

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9A789D37, "sceSsl_lib_9A789D37" )]
		int sceSsl_lib_9A789D37(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9274BFE3, "SSL_library_init_custom" )]
		int SSL_library_init_custom(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCF78BEFB, "SSL_library_cleanup" )]
		int SSL_library_cleanup(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x88A96ED0, "BIO_clear_retry_flags" )]
		int BIO_clear_retry_flags(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x624CB314, "BIO_copy_next_retry" )]
		int BIO_copy_next_retry(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9566709F, "BIO_ctrl" )]
		int BIO_ctrl(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3E040FD4, "BIO_free" )]
		int BIO_free(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x959557F5, "BIO_free_all" )]
		int BIO_free_all(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x558CAA41, "BIO_get_cb" )]
		int BIO_get_cb(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEC4A18D7, "sceSsl_lib_EC4A18D7" )]
		int sceSsl_lib_EC4A18D7(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCA6A3C74, "BIO_get_retry_flags" )]
		int BIO_get_retry_flags(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4A6078CF, "BIO_get_retry_reason" )]
		int BIO_get_retry_reason(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAFD1D124, "BIO_gets" )]
		int BIO_gets(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0FD2F220, "BIO_method_name" )]
		int BIO_method_name(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x19A0DB42, "BIO_method_type" )]
		int BIO_method_type(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5DED429A, "BIO_new" )]
		int BIO_new(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x58352068, "BIO_new_mem" )]
		int BIO_new_mem(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x28B4DE33, "BIO_new_socket" )]
		int BIO_new_socket(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x732F4E44, "BIO_pop" )]
		int BIO_pop(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3454E622, "BIO_printf" )]
		int BIO_printf(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1EC64594, "BIO_push" )]
		int BIO_push(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x675AC5AA, "BIO_read" )]
		int BIO_read(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1BCA32E3, "BIO_retry_type" )]
		int BIO_retry_type(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x26A7CF72, "BIO_set_cb" )]
		int BIO_set_cb(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xECE07B61, "sceSsl_lib_ECE07B61" )]
		int sceSsl_lib_ECE07B61(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8A2F6963, "BIO_set_retry_read" )]
		int BIO_set_retry_read(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEE624680, "BIO_set_retry_special" )]
		int BIO_set_retry_special(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x067173A8, "BIO_set_retry_write" )]
		int BIO_set_retry_write(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xABCD28B8, "BIO_should_io_special" )]
		int BIO_should_io_special(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE3C30923, "BIO_should_read" )]
		int BIO_should_read(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9052D21A, "BIO_should_retry" )]
		int BIO_should_retry(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x089FF1F1, "BIO_should_write" )]
		int BIO_should_write(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCED07EAE, "BIO_write" )]
		int BIO_write(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5E5C873A, "CRYPTO_free" )]
		int CRYPTO_free(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE5FC4992, "CRYPTO_free_locked" )]
		int CRYPTO_free_locked(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x69105335, "sceSsl_lib_69105335" )]
		int sceSsl_lib_69105335(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6CDEB823, "CRYPTO_get_id_cb" )]
		int CRYPTO_get_id_cb(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x064F5DF3, "CRYPTO_get_locked_mem_functions" )]
		int CRYPTO_get_locked_mem_functions(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6BA650E4, "sceSsl_lib_6BA650E4" )]
		int sceSsl_lib_6BA650E4(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB0B90785, "CRYPTO_get_mem_functions" )]
		int CRYPTO_get_mem_functions(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2FE7BB42, "CRYPTO_get_time_cb" )]
		int CRYPTO_get_time_cb(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD84E2411, "CRYPTO_lock" )]
		int CRYPTO_lock(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0861D934, "CRYPTO_malloc" )]
		int CRYPTO_malloc(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6053B79C, "CRYPTO_malloc_locked" )]
		int CRYPTO_malloc_locked(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x64AA4ED6, "sceSsl_lib_64AA4ED6" )]
		int sceSsl_lib_64AA4ED6(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5AD14F0B, "CRYPTO_set_id_cb" )]
		int CRYPTO_set_id_cb(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA9191C17, "sceSsl_lib_A9191C17" )]
		int sceSsl_lib_A9191C17(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA2619268, "CRYPTO_set_mem_functions" )]
		int CRYPTO_set_mem_functions(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9D8A385E, "CRYPTO_thread_id" )]
		int CRYPTO_thread_id(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA7BED83B, "CRYPTO_time" )]
		int CRYPTO_time(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x047AB6BB, "CRYPTO_time_cmp" )]
		int CRYPTO_time_cmp(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9EF66756, "CRYPTO_time_export" )]
		int CRYPTO_time_export(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE7BFC0EF, "CRYPTO_time_import" )]
		int CRYPTO_time_import(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x665D02F6, "CRYPTO_time_offset" )]
		int CRYPTO_time_offset(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x04E677AF, "ERR_clear_error" )]
		int ERR_clear_error(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x101D418A, "ERR_free_strings" )]
		int ERR_free_strings(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE78AD94D, "ERR_get_error" )]
		int ERR_get_error(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x47F7B97F, "ERR_peek_error" )]
		int ERR_peek_error(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9CDCA0D7, "ERR_put_error" )]
		int ERR_put_error(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD8C05497, "ERR_remove_state" )]
		int ERR_remove_state(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7317C1BC, "EVP_PKEY_free" )]
		int EVP_PKEY_free(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFAF333D1, "sceSsl_lib_FAF333D1" )]
		int sceSsl_lib_FAF333D1(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0217E73C, "PEM_ASN1_read_bio" )]
		int PEM_ASN1_read_bio(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA920B39E, "d2i_PrivateKey" )]
		int d2i_PrivateKey(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x21F2A35C, "RAND_bytes" )]
		int RAND_bytes(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x56E19CEB, "RAND_cleanup" )]
		int RAND_cleanup(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA1C3AB31, "RAND_default_method" )]
		int RAND_default_method(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4AE72675, "RAND_get_rand_method" )]
		int RAND_get_rand_method(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDEA6EDB2, "RAND_seed" )]
		int RAND_seed(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1C2728A5, "RAND_set_rand_method" )]
		int RAND_set_rand_method(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA87C65C7, "sceSsl_lib_A87C65C7" )]
		int sceSsl_lib_A87C65C7(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x72D77445, "sceSsl_lib_72D77445" )]
		int sceSsl_lib_72D77445(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6E1E9E0D, "sceSsl_lib_6E1E9E0D" )]
		int sceSsl_lib_6E1E9E0D(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8D484361, "sceSsl_lib_8D484361" )]
		int sceSsl_lib_8D484361(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6443D0EE, "sceSsl_lib_6443D0EE" )]
		int sceSsl_lib_6443D0EE(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7D0E41F4, "sceSsl_lib_7D0E41F4" )]
		int sceSsl_lib_7D0E41F4(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x338C8FDD, "sceSsl_lib_338C8FDD" )]
		int sceSsl_lib_338C8FDD(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xED95A457, "sceSsl_lib_ED95A457" )]
		int sceSsl_lib_ED95A457(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD579CE04, "sceSsl_lib_D579CE04" )]
		int sceSsl_lib_D579CE04(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5040E836, "sceSsl_lib_5040E836" )]
		int sceSsl_lib_5040E836(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4C75B8E9, "sceSsl_lib_4C75B8E9" )]
		int sceSsl_lib_4C75B8E9(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBE4A711C, "sceSsl_lib_BE4A711C" )]
		int sceSsl_lib_BE4A711C(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x45514AE3, "sceSsl_lib_45514AE3" )]
		int sceSsl_lib_45514AE3(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x90BCFCE2, "sceSsl_lib_90BCFCE2" )]
		int sceSsl_lib_90BCFCE2(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2B7BD166, "sceSsl_lib_2B7BD166" )]
		int sceSsl_lib_2B7BD166(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9307DE23, "sceSsl_lib_9307DE23" )]
		int sceSsl_lib_9307DE23(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEDBE00D6, "sceSsl_lib_EDBE00D6" )]
		int sceSsl_lib_EDBE00D6(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x737BED1C, "sceSsl_lib_737BED1C" )]
		int sceSsl_lib_737BED1C(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE11361C2, "sceSsl_lib_E11361C2" )]
		int sceSsl_lib_E11361C2(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC6ED5C87, "sceSsl_lib_C6ED5C87" )]
		int sceSsl_lib_C6ED5C87(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x37C7B76C, "sceSsl_lib_37C7B76C" )]
		int sceSsl_lib_37C7B76C(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC1A2E14B, "sceSsl_lib_C1A2E14B" )]
		int sceSsl_lib_C1A2E14B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6038DEDE, "sceSsl_lib_6038DEDE" )]
		int sceSsl_lib_6038DEDE(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCFA2F8EB, "sceSsl_lib_CFA2F8EB" )]
		int sceSsl_lib_CFA2F8EB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB1E75EA9, "sceSsl_lib_B1E75EA9" )]
		int sceSsl_lib_B1E75EA9(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4BE9030A, "sceSsl_lib_4BE9030A" )]
		int sceSsl_lib_4BE9030A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x05E019A4, "sceSsl_lib_05E019A4" )]
		int sceSsl_lib_05E019A4(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x341843AB, "sceSsl_lib_341843AB" )]
		int sceSsl_lib_341843AB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB87FFE37, "sceSsl_lib_B87FFE37" )]
		int sceSsl_lib_B87FFE37(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9299C6CE, "sceSsl_lib_9299C6CE" )]
		int sceSsl_lib_9299C6CE(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x12C7718F, "sceSsl_lib_12C7718F" )]
		int sceSsl_lib_12C7718F(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0A241EDE, "sceSsl_lib_0A241EDE" )]
		int sceSsl_lib_0A241EDE(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x999D7614, "sceSsl_lib_999D7614" )]
		int sceSsl_lib_999D7614(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1E257243, "sceSsl_lib_1E257243" )]
		int sceSsl_lib_1E257243(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1B93AD27, "sceSsl_lib_1B93AD27" )]
		int sceSsl_lib_1B93AD27(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA739BFD9, "sceSsl_lib_A739BFD9" )]
		int sceSsl_lib_A739BFD9(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0FF7BA68, "sceSsl_lib_0FF7BA68" )]
		int sceSsl_lib_0FF7BA68(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB4D78E98, "SSL_CTX_ctrl" )]
		int SSL_CTX_ctrl(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4E0F2CCC, "SSL_CTX_flush_sessions" )]
		int SSL_CTX_flush_sessions(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x588F2FE8, "SSL_CTX_free" )]
		int SSL_CTX_free(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x909E9D31, "sceSsl_lib_909E9D31" )]
		int sceSsl_lib_909E9D31(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE14101E4, "SSL_CTX_get_quiet_shutdown" )]
		int SSL_CTX_get_quiet_shutdown(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x795B9EDF, "SSL_CTX_get_timeout" )]
		int SSL_CTX_get_timeout(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFB8273FE, "SSL_CTX_new" )]
		int SSL_CTX_new(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB81AD643, "SSL_CTX_remove_session" )]
		int SSL_CTX_remove_session(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD5ABC603, "sceSsl_lib_D5ABC603" )]
		int sceSsl_lib_D5ABC603(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x529A9477, "sceSsl_lib_529A9477" )]
		int sceSsl_lib_529A9477(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBF55C31C, "SSL_CTX_set_client_cert_cb" )]
		int SSL_CTX_set_client_cert_cb(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x42DF4592, "SSL_CTX_set_quiet_shutdown" )]
		int SSL_CTX_set_quiet_shutdown(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4214B7AF, "SSL_CTX_set_timeout" )]
		int SSL_CTX_set_timeout(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAEBF278B, "SSL_CTX_set_verify" )]
		int SSL_CTX_set_verify(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBDAD0841, "SSL_SESSION_free" )]
		int SSL_SESSION_free(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x86D0771E, "sceSsl_lib_86D0771E" )]
		int sceSsl_lib_86D0771E(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x54A7D8F3, "SSL_clear" )]
		int SSL_clear(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x84833472, "SSL_free" )]
		int SSL_free(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE92302C5, "SSL_get_SSL_CTX" )]
		int SSL_get_SSL_CTX(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x340F8829, "sceSsl_lib_340F8829" )]
		int sceSsl_lib_340F8829(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x92FA8B34, "sceSsl_lib_92FA8B34" )]
		int sceSsl_lib_92FA8B34(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7BDAF864, "sceSsl_lib_7BDAF864" )]
		int sceSsl_lib_7BDAF864(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD49A570C, "SSL_get_default_timeout" )]
		int SSL_get_default_timeout(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB3B04C58, "SSL_get_error" )]
		int SSL_get_error(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBB1564A3, "SSL_get_ex_data_X509_STORE_CTX_idx" )]
		int SSL_get_ex_data_X509_STORE_CTX_idx(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x95E346AB, "SSL_get_info_cb" )]
		int SSL_get_info_cb(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6A19E0AA, "SSL_get_quiet_shutdown" )]
		int SSL_get_quiet_shutdown(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2E2E2B09, "SSL_get_rbio" )]
		int SSL_get_rbio(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEF5A4BD4, "SSL_get_session" )]
		int SSL_get_session(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD1205C58, "SSL_get_shutdown" )]
		int SSL_get_shutdown(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAA3B27CF, "SSL_get_verify_result" )]
		int SSL_get_verify_result(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x868C15DF, "SSL_get_wbio" )]
		int SSL_get_wbio(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEBFB0E3C, "SSL_new" )]
		int SSL_new(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE7C29542, "SSL_read" )]
		int SSL_read(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA8F0AD39, "SSL_reuse" )]
		int SSL_reuse(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBE423CD1, "sceSsl_lib_BE423CD1" )]
		int sceSsl_lib_BE423CD1(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAE3986D3, "sceSsl_lib_AE3986D3" )]
		int sceSsl_lib_AE3986D3(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9CB1A62F, "sceSsl_lib_9CB1A62F" )]
		int sceSsl_lib_9CB1A62F(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB9C8CCE6, "SSL_set_bio" )]
		int SSL_set_bio(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x80608663, "SSL_set_connect_state" )]
		int SSL_set_connect_state(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC270B3A8, "SSL_set_info_cb" )]
		int SSL_set_info_cb(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4AF959E3, "SSL_set_quiet_shutdown" )]
		int SSL_set_quiet_shutdown(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x69F1B678, "SSL_set_session" )]
		int SSL_set_session(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x86D4034D, "SSL_set_shutdown" )]
		int SSL_set_shutdown(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5605C9FE, "SSL_set_verify_result" )]
		int SSL_set_verify_result(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3E3133D6, "SSL_shutdown" )]
		int SSL_shutdown(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x104F749D, "SSL_state" )]
		int SSL_state(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8390B321, "SSL_use_PrivateKey" )]
		int SSL_use_PrivateKey(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC0ABBB57, "SSL_use_certificate" )]
		int SSL_use_certificate(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x57F2E960, "SSL_version" )]
		int SSL_version(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC97D0510, "SSL_want" )]
		int SSL_want(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB7CA8717, "SSL_write" )]
		int SSL_write(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB40D11EA, "SSLv3_client_method" )]
		int SSLv3_client_method(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9266C0D5, "sceSsl_lib_9266C0D5" )]
		int sceSsl_lib_9266C0D5(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8B78A6C0, "sceSsl_lib_8B78A6C0" )]
		int sceSsl_lib_8B78A6C0(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA2CE8DCD, "SSL_CIPHER_get_name" )]
		int SSL_CIPHER_get_name(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x88897D26, "SSL_get_current_cipher" )]
		int SSL_get_current_cipher(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x09C45275, "EVP_PKEY_new" )]
		int EVP_PKEY_new(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBE5FF9F1, "PEM_do_header" )]
		int PEM_do_header(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB99D89E2, "sceSsl_lib_B99D89E2" )]
		int sceSsl_lib_B99D89E2(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - DBA4DA08 */
