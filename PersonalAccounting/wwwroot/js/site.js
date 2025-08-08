$(function(){
    // Re-parse unobtrusive validation for dynamically loaded forms
    $(document).on('shown.bs.modal', '#modalContainer', function(){
        const form = $(this).find('form');
        if(form.length){
            form.removeData('validator');
            form.removeData('unobtrusiveValidation');
            $.validator.unobtrusive.parse(form);
        }
    });
});