
require(["jquery"], function($) {
    //the jquery.alpha.js and jquery.beta.js plugins have been loaded.
    $(window).resize(function() {
		$('.my-width').text('oim is ' + $(window).width() + 'px wide. '); 
	});


	$(function(){
		if($('.carousel').length) {
			require(["jquery", "bootstrap-transition", "bootstrap-carousel"]);
		}
	});

	$(function(){
		if($('.dropdown').length) {
			require(["jquery", "bootstrap-transition", "bootstrap-dropdown"]);
		}
	});

	$(function(){
		if($('.collapse').length){
			require(["jquery", "bootstrap-transition", "bootstrap-collapse"]);	
		}
	});

	$(function(){
		if($('.dropdown').length){
			require(["jquery", "bootstrap-transition", "bootstrap-dropdown"]);	
		}
	});

	

});

