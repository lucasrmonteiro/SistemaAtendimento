jQuery(document).ready(function () {

});

function alterarPerfilUsu () {
	
	var id_usu = jQuery('#hiddenIdUsu').val();
	var id_fluxo = jQuery('#fluxo').val();
	var idsperfils = "";
	jQuery.each(jQuery('.chPerfilSelecionado'), function(index, val) {
	  if(jQuery(this).is(':checked')){
	  	idsperfils += jQuery(this).val() + "|"
	  }
	});

	if(id_fluxo != "" && idsperfils != ""){
		$.ajax({
			url: '../Usuario/alteraPerfilFluxoUsu',
			type: 'GET',
			dataType: 'json',
			data: {id_fluxo: id_fluxo ,id_usu: id_usu ,idsperfils: idsperfils},
		})
		.then(function(poJson) {
			 $.unblockUI();
			 
			 alertify.alert('Dados Alterados com Sucesso.');
			
		})		
	}else{
		alertify.alert('Selecione o FLuxo eos Perfis.');
	}
}