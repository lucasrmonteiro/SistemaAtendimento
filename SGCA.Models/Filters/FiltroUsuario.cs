using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SGCA.Models.Validators;

namespace SGCA.Models.Filters
{
    public class FiltroUsuario
    {
        [StringLength(100, ErrorMessage = "O campo Nome excedeu o tamanho limite de caracteres!")]
        public string Dsc_nome  { get; set; }

        public int Empresa { get; set; }

        [StringLength(14, ErrorMessage = "O campo Cpf excedeu o tamanho limite de caracteres!")]
        [CpfValidator(ErrorMessage = "CPF Inválido")]
        public string Dsc_cpf { get; set; }

        [StringLength(100, ErrorMessage = "O campo Login excedeu o tamanho limite de caracteres!")]
        public string Dsc_login { get; set; }

        public int Perfil { get; set; }

        public int Status { get; set; }
    }
}