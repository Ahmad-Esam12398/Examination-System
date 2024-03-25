document.getElementsByTagName("select")[0].addEventListener("change", () => {

    let selectList = document.getElementsByTagName("select")[0];
    var selectedOption = selectList.options[selectList.selectedIndex];

    var courseName = selectedOption.textContent;

    let rows = document.getElementsByTagName("tr");

    for (let i = 1; i < rows.length; i++) {
        let qCourse = rows[i].getElementsByTagName("td")[1].innerText;
        if (courseName === "All Courses" || qCourse === courseName)
            rows[i].style.display = '';
        else
            rows[i].style.display = 'none';
    }

});//end of change select list
