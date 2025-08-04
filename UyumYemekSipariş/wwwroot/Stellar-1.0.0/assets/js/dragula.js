(function($) {
  'use strict';
  var iconTochange;
  dragula([document.getElementById("dragula-left"), document.getElementById("dragula-right")]);
  dragula([document.getElementById("profile-list-left"), document.getElementById("profile-list-right")]);
  dragula([document.getElementById("dragula-event-left"), document.getElementById("dragula-event-right")])
    .on('drop', function(el) {
      console.log($(el));
      iconTochange = $(el).find('.icon-check');
      if (iconTochange.hasClass('text-warning')) {
        iconTochange.removeClass('text-warning').addClass('text-success');
      } else if (iconTochange.hasClass('text-success')) {
        iconTochange.removeClass('text-success').addClass('text-warning');
      }
    })
})(jQuery);