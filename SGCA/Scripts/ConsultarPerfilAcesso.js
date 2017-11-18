jQuery(document).ready(function() {
	
});


function consultaPerilLogin () {

	var login = jQuery('#login').val();
	var perfil = jQuery('#Id_perfil').val();
	var fluxo = jQuery('#Id_fluxo_atendimento').val();

	if(login != ""){
		$.blockUI({
	        message: '<h2><img src="@Url.Content("~/Content/Images/load.gif")" /></br>Carregando, por favor aguarde...</h2>',
	        css: {
	            border: 'none', padding: '15px', backgroundColor: '#000', '-webkit-border-radius': '10px',
	            '-moz-border-radius': '10px', opacity: .5, color: '#fff'
	        }
	    });
	    setTimeout($.unblockUI, 99000);
		$.ajax({
			url: '../Usuario/carregaPerfilFluxoUsuario',
			type: 'GET',
			dataType: 'json',
			data: {login: login ,id_perfil: perfil ,id_fluxo_atendimento: fluxo},
		})
		.then(function(poJson) {
			 $.unblockUI();
			 
			 if(poJson.length > 0){
				jQuery('#table-consulta-perfil').show();

				var linha = "";

				jQuery.each(poJson, function(index, json) {
				  linha +="<tr>";
				  linha +="<td>"+json.nome_usuario+"</td>"
				  linha +="<td>"+json.desc_perfil+"</td>"
				  linha +="<td>"+json.desc_fluxo_Atendimento+"</td>"
				  linha +="</tr>";


				});

				jQuery('#consulta_uusario_perfil').append(linha)
			 }else{
			 	alertify.alert("Usuário incorreto ou não cadastrado");
			 }
		})		
	}else{
		 alertify.alert('Insira o TR do Usuário.');
	}
}

function goToAlterar () {
	
	var usu = jQuery('#login').val()

	window.location.href="/Usuario/AlterarPerfilUsuario?login_consulta="+usu;
}