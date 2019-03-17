//переменньіе для отслеживания состояния прохождения валидации полями
var aVal = false;//валидация возраста
var eVal = false;//вадидация email-а
var pVal = false;//валидация пароля
var cVal = false;//валидация почтового кода

//сообщения непрохождения валидации
var ageMess = "You must be at least 18 year old";
var emptyMess = "This field can't be empty";
var invalidEmailMessage =  "Please include an '@' in the email address";

var $inputYear = $("._inputYear");

var now = new Date();
var nowYear = now.getFullYear();

//заполнение віпадающего списка дат
for (var i = 1950; i<=nowYear; i++){
	var inStr = "<option>" + i + "</option>";
	$(inStr).appendTo($inputYear);
}

$inputYear.val(nowYear);

//назначение обработчика собьітия для осуществления анимации
$("._move").on("click", function(){
	if(event.target.classList.contains('_forMove')) {
		$(this).addClass("row-reg_icons__move");
	} else if(event.target.classList.contains('_forBackMove') ){
		$(this).removeClass("row-reg_icons__move");
	}
});

var $formReg = $("._formReg");

//візов собьітия отправки форм=ьі
$formReg.submit(function(){
	console.log("Form is valid!");
});

//назначение обработчика собьітия потери фокуса для осуществления валидации
$formReg.on("focusout", "._inputValid", validation);
//назначение обработчика собьітия фокусировки для осуществления валидации
$formReg.on("focusin", "._inputValid", clearInvalidEffect);

//назначение обработчика собьітия нажатия на кнопку отправку формьі
$("._submitReg").on("click", function (){
	$inputs = $("._inputValid");
	console.log(aVal + eVal + pVal + cVal);
	if (aVal==false || eVal==false || pVal==false || cVal==false) {
		$inputs.each(validation);
	} else if (aVal && eVal && pVal && cVal) {
			console.log("SUBMIT");
			$formReg.submit();
	}
});

//назначение обработчика собьітия наведения на подсказку
$("._questionReg").on("mouseenter", function() {
	$(this).siblings("._rowTooltip").slideDown(500);
});

//назначение обработчика собьітия потери наведения на подсказку
$("._questionReg").on("mouseleave", function() {
	$(this).siblings("._rowTooltip").slideUp(500);
});

//функция снятия визуального вьіделение непрохождения валидации
function clearInvalidEffect (){
	$(this).removeClass("row-reg_input__invalid");
}

//функция визуального віделения непрохождения валидации
function markInvalid ($elem, message){
	$elem.addClass("row-reg_input__invalid")
	.siblings("._rowTooltip").queue(function (next) {
		$elem.siblings("._rowTooltip").find("._tooltipText").text(message);
		next();
	});
	$elem.siblings("._rowTooltip").fadeIn(1000);
}

//фукция валидирования полей формі
function validation () {
	
	var flgValid = false;	

	$(this).siblings("._rowTooltip").fadeOut(200);

	if ($(this).hasClass("_inputYear")) {
		flgValid = ageValidation($(this));
		aVal = flgValid;
		revisionMark(aVal, $(this), ageMess);
	} else {
		var flgValid = emptyValidation($(this));

		if($(this).hasClass("_inputEmail") && flgValid) {
			flgValid = validationEmail($(this));
			eVal = flgValid;
			revisionMark(eVal, $(this), invalidEmailMessage);
		} else if($(this).hasClass("_inputPass")){
			pVal = flgValid;
			revisionMark(pVal, $(this), emptyMess);
		} else if($(this).hasClass("_inputPostCode")){
			cVal = flgValid;
			revisionMark(cVal, $(this), emptyMess);
		} else if($(this).hasClass("_inputEmail")){
			eVal = flgValid;
			revisionMark(eVal, $(this), emptyMess);
		} 
	}

	
}

//функция проверки необходимости очистки визуального вьіделения
function revisionMark (flagVal, $elem, message){
	if (!flagVal) {
		markInvalid($elem, message);
	}
}

//функция валидирования возраста
function ageValidation ($elem) {
	
	var now = new Date();
	var nowYear = now.getFullYear();

	var inputYear = +$elem.val();
	
	if ((nowYear - inputYear) < 18) {
		
		return false;
	} else {
		return true;
	}
}

//функция валирования на пустое поле
function emptyValidation($elem){
	
	if (!valExist($elem)) {
		
		return false;
	} else {
		return true;
	}
}

//фукция валидирования email-а
function validationEmail($elem){
	
	var str = $elem.val();
	var dogFlag = str.indexOf("@");
	console.log(dogFlag);
	if(dogFlag == -1) {
		
		return false;
	} else {
		return true;
	}
}

//функция проверки заполненности поля
function valExist ($elem) {
	if ($elem.val() != "") {
		return true;
	}
	else return false;
}
