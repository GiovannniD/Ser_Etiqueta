$("#tieneSucursal").prop('checked', true);
$(function () {
    alertify.defaults.theme.ok = "btn btn-primary";
    alertify.defaults.theme.cancel = "btn btn-danger";
    $('.select2').select2()
    //Initialize Select2 Elements
    $('.select2bs4').select2({
        theme: 'bootstrap4'
    })
   // loadTable();
    Form();
    

    loadDepartamento();
    getMunicipio(1,0);
    loadIdEmpresas()
})

function selectRefresh() {
    $('.select2bs4').select2({
        //-^^^^^^^^--- update here
        theme: 'bootstrap4',
        width: '100%'
    });
}
function loadDepartamento() {
    $('#idDepartamento').html("");

    $.ajax({
        url: urlDepartamento, // Url
        data: "",
        type: "get"  // Verbo HTTP
    })
        // Se ejecuta si todo fue bien.
        .done(function (result) {
            console.log(result)
            if (result != null) {

                var outObjA = JSON.parse(JSON.stringify(result));

                for (var i = 0; i < outObjA.length; i++) {
                    var jsonData = outObjA[i];
                    $('#idDepartamento').append($("<option />").val(jsonData.keyDepartamento).text(jsonData.descripcionDep));
                    $('#idDepartamentoSucursal').append($("<option />").val(jsonData.keyDepartamento).text(jsonData.descripcionDep));
                 //   console.log(jsonData.descripcionDep)
                    //$('#cargo_2').append($("<option />").val(jsonData.cargo).text(jsonData.cargo));
                    // console.log(jsonData.id);
                }
            }
        })
        // Se ejecuta si se produjo un error.
        .fail(function (xhr, status, error) {
            // Mostramos un mensaje de error.
            //    $("#ErrorAlert").show("slow").delay(2000).hide("slow");

            // Escondemos el Ajax Loader
            //  $("#AjaxLoader").hide("slow");

            // Habilitamos el botón de Submit
            //  $("#SubmitBtn").prop("disabled", false);
        })
        // Hacer algo siempre, haya sido exitosa o no.
        .always(function () {

        });

}



function getMunicipio(keyDepartamento,caso) {
    $('#idMunicipio').html("");
   
    if (caso == 1) { $('#idMunicipio').html("");}
       
    else if (caso == 2) { $('#idMunicipioSucursal').html("");}
       
    else {
        $('#idMunicipio').html("");
        $('#idMunicipioSucursal').html("");}

    $.ajax({
        url: urlMunicipio, // Url
        data: { keyDepto: keyDepartamento },
        type: "post"  // Verbo HTTP
    })
        // Se ejecuta si todo fue bien.
        .done(function (result) {
            console.log(result)
            if (result != null) {
                var outObjA = JSON.parse(JSON.stringify(result));

                for (var i = 0; i < outObjA.length; i++) {
                    var jsonData = outObjA[i];
                    if (caso==1)
                        $('#idMunicipio').append($("<option />").val(jsonData.keyMunicipio).text(jsonData.descripcionMun));
                    else if(caso==2)
                        $('#idMunicipioSucursal').append($("<option />").val(jsonData.keyMunicipio).text(jsonData.descripcionMun));
                    else 
                        $('#idMunicipio').append($("<option />").val(jsonData.keyMunicipio).text(jsonData.descripcionMun));
                    $('#idMunicipioSucursal').append($("<option />").val(jsonData.keyMunicipio).text(jsonData.descripcionMun));
                   // console.log(jsonData.descripcionDep)
                    //$('#cargo_2').append($("<option />").val(jsonData.cargo).text(jsonData.cargo));
                    // console.log(jsonData.id);
                }
            }
        })
        // Se ejecuta si se produjo un error.
        .fail(function (xhr, status, error) {
            // Mostramos un mensaje de error.
            //    $("#ErrorAlert").show("slow").delay(2000).hide("slow");

            // Escondemos el Ajax Loader
            //  $("#AjaxLoader").hide("slow");

            // Habilitamos el botón de Submit
            //  $("#SubmitBtn").prop("disabled", false);
        })
        // Hacer algo siempre, haya sido exitosa o no.
        .always(function () {

        });

}


function loadIdEmpresas() {
    $('#idEmpresaSucursal').html("");


$.ajax({
    url: urlLoad, // Url
  //  data: { keyDepto: keyDepartamento },
    type: "post"  // Verbo HTTP
})
    // Se ejecuta si todo fue bien.
    .done(function (result) {
        console.log(result)
        if (result != null) {
            var outObjA = JSON.parse(JSON.stringify(result));

            for (var i = 0; i < outObjA.length; i++) {
                var jsonData = outObjA[i];
                $('#idEmpresaSucursal').append($("<option />").val(jsonData.idEmpresa).text(jsonData.idSersa+'-'+jsonData.nombreEmpresa));
             


                // console.log(jsonData.descripcionDep)
                //$('#cargo_2').append($("<option />").val(jsonData.cargo).text(jsonData.cargo));
                // console.log(jsonData.id);
            }
        }
    })
    // Se ejecuta si se produjo un error.
    .fail(function (xhr, status, error) {
        // Mostramos un mensaje de error.
        //    $("#ErrorAlert").show("slow").delay(2000).hide("slow");

        // Escondemos el Ajax Loader
        //  $("#AjaxLoader").hide("slow");

        // Habilitamos el botón de Submit
        //  $("#SubmitBtn").prop("disabled", false);
    })
    // Hacer algo siempre, haya sido exitosa o no.
    .always(function () {

    });

}