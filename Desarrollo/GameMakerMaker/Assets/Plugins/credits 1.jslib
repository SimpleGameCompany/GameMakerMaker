mergeInto(LibraryManager.library,{

GoCredits : function () {
    $('html, body').animate({
        scrollTop: $("#team_section").offset().top + 20
    }, 1000);
},

SetSpanish : function (){
    ponerEspañol();
},

SetEnglish : function (){
    ponerIngles();
}

});