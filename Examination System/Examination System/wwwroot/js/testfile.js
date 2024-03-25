$(document).on("change", "#assignExamModal > div > div > div.modal-body > table > tbody > tr:nth-child(1) > td:nth-child(2) > select", function () {
    var BranchId = $("#assignExamModal > div > div > div.modal-body > table > tbody > tr:nth-child(1) > td:nth-child(2) > select").val();
    var crsId = $('#AssignFinal').data('crsId');
    $.ajax({
        url: "/Instrucotr/Read_Track_From_Instructor_Course_BranchResultsAsync",
        type: "GET",
        data: { BranchId: BranchId, crsId: crsId },
        success: function (data) {
            $('#assignExamModal > div > div > div.modal-body > table > tbody > tr:nth-child(2) > td:nth-child(2) > select')
        }
    })
})