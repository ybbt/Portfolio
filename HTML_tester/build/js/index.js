$(function () {
	
	$("._clickToPopup").click(function () {
		console.log('ok');
		$("#showSlider").fadeIn();
		
		$("body").css("overflow", "hidden");
		slick.slick('refresh');
	});
	
	$("#overlaySlider").click(function (e) {
		
		if($(e.target).closest('.show_main').length) {
	  		return;
		}
		$("#showSlider").fadeOut();
		
		$("body").css("overflow", "auto");
	});


	$("#clickToPopupVideo").click(function () {
		
		$("#showVideo").fadeIn();
		
		$("body").css("overflow", "hidden");
		slick.slick('refresh');
	});
	
	$("#overlayVideo").click(function (e) {
		
		if($(e.target).closest('.show_main').length) {
	  		return;
		}
		$("#showVideo").fadeOut();
		
		$("body").css("overflow", "auto");
	});


	var slick = $('._sliderFor').slick({
		slidesToShow: 1,
		slidesToScroll: 1,
		arrows: false,
		fade: true,
		asNavFor: '._sliderNav',
		dots: true,
		
	});
	$('._sliderNav').slick({
		slidesToShow: 1,
		slidesToScroll: 1,
		asNavFor: '._sliderFor',
		dots: false,
		
		focusOnSelect: true,
		
		nextArrow: '._nextArrow',
		prevArrow: '._prevArrow',
	});

});

