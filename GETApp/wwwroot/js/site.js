

$(() => {
    LoadResData();

    var connection = new signalR.HubConnectionBuilder().withUrl("/signalrServer").build();
    connection.start();
    connection.on("LoadReservations", function () {
        LoadResData();
    })
    LoadResData();

    function LoadResData() {
        var tr = '';
        $.ajax({
            url: '/Customer/Reservations/GetReservations',
            method: 'GET',
            success: (result) => {
                $.each(result, (k, v) => {
                    tr += `<tr>
                        <td> ${v.Name}</td>
                        <td> ${v.PhoneNumber}</td>
                        <td> ${v.DateTime}</td>
                        <td> ${v.Table}</td>
                        <td> ${v.AdditionalComment}</td>
                        
                      </tr>`

                })

                $("#tableBody").html(tr);

            },
            error: (error) => {
                console.log(error)
            }



        });


    }

})