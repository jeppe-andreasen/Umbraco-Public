(function ($){
	$(function(){
		$('#classswitch').live('click', function(e){
			e.preventDefault();
			console.log('click');
			$('body').toggleClass('od');
		})
	})
})(jQuery)