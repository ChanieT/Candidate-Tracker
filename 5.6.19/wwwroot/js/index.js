$(() => {
    //setInterval(() => {
    //    updateAmounts();
    //}, 1000);

    $("#confirm-btn").on('click', function () {
        const id = $("#candidateId").val();
        $.post('/home/confirm', { id }, function () {
            console.log("boo");
            updateAmounts();
        });
    });

    $("#decline-btn").on('click', function () {
        const id = $("#candidateId").val();
        $.post('/home/decline', { id }, function () {
            updateAmounts();
        });
    });

    function updateAmounts() {
        pendingAmt();
        confirmedAmt();
        declinedAmt();
    }

    function pendingAmt() {
        $.get('/home/getpending', function (result) {
            $("#pending-vb").text(result);
        });
    }

    function confirmedAmt() {
        $.get('/home/getconfirmed', function (result) {
            $("#confirmed-vb").text(result);
        });
    }

    function declinedAmt() {
        $.get('/home/getdeclined', function (result) {
            $("#declined-vb").text(result);
        });
    }
});