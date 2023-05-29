// Regex - Inputs somente Númericos
var inputs = document.getElementsByTagName('input');

for (var i = 0; i < inputs.length; i++) {
	var input = inputs[i];

	input.setAttribute('oninput', "this.value = this.value.replace(/[^0-9.]/g, '').replace(/(\\..*?)\\..*/g, '$1').replace(/[^\\d]+/g,'');");
}