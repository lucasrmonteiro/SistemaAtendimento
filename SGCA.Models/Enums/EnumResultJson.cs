using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGCA.Models.Enums
{
    public enum EnumResultJson
    {
        ERRO_PELO_MENOS_UM_CAMPO_E_NECESSARIO_PARA_A_CONSULTA = 1,
        ERRO_DADOS_NAO_ENCONTRADOS = 2,
        ERRO_CPF_INVALIDO = 3,
        ERRO_NOME_EXCEDEU_TAMANHO_LIMITE = 4,
        ERRO_LOGIN_EXCEDEU_TAMANHO_LIMITE = 5,
        SUCESSO = 6
    }
}