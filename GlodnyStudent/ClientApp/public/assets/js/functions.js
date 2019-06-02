$(document).ready(function() {
    updateContainer();
    
    $(window).resize(function() {
      updateContainer();
    });
  });

  function updateContainer() {
    if($(window).innerWidth() < 900) 
    {
      $(".topnav").hide();

      $("a#menuIcon").click(function() {
          $(".topnav").fadeToggle();
          e.preventDefault();
      });
    }
    else {
      $(".topnav").show();
    }
  }