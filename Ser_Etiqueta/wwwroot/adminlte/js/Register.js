

$(function () {
    $('.select2').select2()
    //Initialize Select2 Elements
    $('.select2bs4').select2({
        theme: 'bootstrap4'
    })

        


});

function getSucursal(idEmpresa) {
  //  alert(idEmpresa)
    $("#idSucursal").html("")
    $.ajax({
        url: urlGetEmpresa, // Url
        data: { idEmpresa: idEmpresa },
        type: "post",
      //  contentType: "application/json; charset=utf-8",// Verbo HTTP
    })
        // Se ejecuta si todo fue bien.
        .done(function (result) {
            console.log(result)
            $('#idSucursal').append($("<option />").val("0").text("No tiene"));
            if (result != null) {
              //  console.log(result)
                var outObjA = JSON.parse(JSON.stringify(result));
                for (var i = 0; i < outObjA.length; i++) {
                    var jsonData = outObjA[i];
                    $('#idSucursal').append($("<option />").val(jsonData.idSucursal).text(jsonData.nombreSucursal));
                  
                }

            }
        })
        // Se ejecuta si se produjo un error.
        .fail(function (xhr, status, error) {

        })
        // Hacer algo siempre, haya sido exitosa o no.
        .always(function () {

        });
}
