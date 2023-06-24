var priceElement = document.getElementById("price");
var priceText = priceElement.textContent.trim();
var unitPrice = parseInt(priceText.replace(/\D/g, ''));


var startDate = localStorage.getItem("startDateStorage");
var endDate = localStorage.getItem("endDateStorage");


var formattedStartDate = formatDate(startDate);
var formattedEndDate = formatDate(endDate);

document.getElementById("timebooking").textContent = formattedStartDate + " → " + formattedEndDate;

function formatDate(dateString) {
  var date = new Date(dateString);
  var hours = date.getHours();
  var minutes = date.getMinutes();
  var day = date.getDate();
  var month = date.getMonth() + 1;
  var year = date.getFullYear();

  hours = addLeadingZero(hours);
  minutes = addLeadingZero(minutes);

  return hours + ":" + minutes + " " + day + "/" + month + "/" + year;
}

function addLeadingZero(number) {
  return number < 10 ? "0" + number : number;
}
document.getElementById("totalDays").textContent = "x" + localStorage.getItem("totalDays") + " ngày";
document.getElementById("totalPrice").textContent = localStorage.getItem("totalPrice") + " ngày";
