var scrollerArray = [
	{
		currSel: '._headerScroll',
		destSel: '._main',
	},
	{
		currSel: '._project',
		destSel: '._mainProject',
	},
	{
		currSel: '._news',
		destSel: '._mainNews',
	},
	{
		currSel: '._contact',
		destSel: '._mainContact',
	},
	{
		currSel: '._aboutUs',
		destSel: '._mainGallery',
	}
]

$(document).ready(function(){
			
	$('._headerConteiner').slick({
		slidesToShow: 1,
		slidesToScroll:1,
		arrows: false,
		fade: true,
		dots: true,
		autoplay: true,
		autoplaySpeed: 4000,
		draggable: true,
	});
	
});

$(document).ready(function(){
			
	$('._mainContainer').slick({
		slidesToShow: 3,
		slidesToScroll:1,
		// arrows: true,
		// // fade: true,
		dots: true,
		// // autoplay: true,
		// // autoplaySpeed: 4000,
		// centerMode: true,
		prevArrow: '<img src="img/service_img/arrow_left.png" class="arr-prev">',
		nextArrow: '<img src="img/service_img/arrow_right.png" class="arr-next">',
		
	});
	
});

function loadMap() {
	// alert('ho-ho-ho');
	var loc = {lat:40.67, lng: -73.94};
	worldMap = new google.maps.Map(document.querySelector('._map'),{
		zoom:16, 
		center:loc, 
		disableDefaultUI: true
	});
	
};

$(function(){
	// $('.header_scroll-img').on('click', function(){
	// 	var N = $('.main').offset().top;
	// 	$('html').animate({
	// 		scrollTop: N,
	// 	}, 1000)
	// })

	scrollerArray.forEach(function(item){
		setClick(item.currSel, item.destSel);
	});

	//setClick('.header_scroll-img', '.main');

	function setClick( currentSel, destinSel){
		$(currentSel).on('click', function(e){
			e.preventDefault();
			var N = $(destinSel).offset().top;
			$('html').animate({
				scrollTop: N,
			}, 1000)
		})
	}
})