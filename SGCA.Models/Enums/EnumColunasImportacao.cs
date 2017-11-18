using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGCA.Models.Enums
{
    public enum EnumIndiceTicket
    {
        //Indices das colunas referentes ao ticket. Classe Ticket
        NUMERO_TICKET =  1,
        FLUXO_ATENDIMENTO,
        ATIVIDADE,
        DATA_CRIACAO,
        DATA_ENCERRAMENTO,
        DEMANDA,
        JUSTIFICATIVA_PRIORIZACAO
    }

    public enum EnumIndiceNota
    {
        //Indices das colunas referentes à nota. Classe Nota
        STATUS_NOTA = 8,
        AREA_NOTA,
        OBSERVACOES,
        MENSAGENS,
        NUMERO_NOTA,
        PENDENCIA_NS,
        TIPO_NOTA,
        STATUS_NOTA_SAP,
        STATUS_NOTA_USUARIO,
        ID_INSTALACAO_NOTA
    }

    public enum EnumIndiceMobilidade
    {
        //Indices das coluna referentes à mobilidade(informação complementar da nota). Classes Nota e Mobilidade
        STATUS_OS = 18,
        SUBCATEGORIA_OS,
        ZONA_ATENDIMENTO,
        NOME_GASISTA,
        REGISTRO_GASISTA,
        VIATURA,
        ENDERECO_MOBILIDADE,
        BAIRRO,
        CIDADE,
        DESCRICAO_MATERIAL,
        QUANTIDADE,
        VALOR,
        
    }

    public enum EnumIndicePendenciaNota
    {
        //Indices das colunas referentes à pendência mobilida(Grupo 2) e A104(grupo 3). Classe PendenciaNota
        SEGMENTO_CLIENTE = 30,
        NUMERO_NOTA_PENDENCIA,
        TIPO_NOTA_PENDENCIA,
        INICIO_DESEJADO,
        INSTALACAO_PENDENCIA,
        STATUS_NOTA_USUARIO_PENDENCIA,
        CENTRAB_RESPON,
        ENDERECO_PENDENCIA,
        NUMERO_APARTAMENTO,
        CIDADE_PENDENCIA,
        DESCRICAO,
        CODIGO_PENDENCIA,
        DESCRICAO_PENDENCIA,
        DATA_CRIACAO_A104,
        MENSAGEM_ERRO,
        CODIGO_AREA,
        DESCRICAO_AREA,
        TEXTO_CODIFICACAO,
        DATA_ENCERRAMENTO_SAP
    }
}