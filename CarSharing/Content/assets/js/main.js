var currentDate = new Date();
var currentDateTime = currentDate.toISOString().slice(0, 16);


document.getElementById("dateTimePickerStart").min = currentDateTime;
document.getElementById("dateTimePickerEnd").min = currentDateTime;

// HÃ m cáº­p nháº­t ngÃ y tráº£ xe
function updateEndDate() {
  var startDate = document.getElementById("dateTimePickerStart").value;
  var endDateInput = document.getElementById("dateTimePickerEnd");
  var startDateTime = new Date(startDate);
  var endDateTime = new Date(endDateInput.value);

  endDateInput.min = startDateTime.toISOString().split(".")[0];

  if (endDateTime.getDate() == startDateTime.getDate() && endDateTime.getMonth() == startDateTime.getMonth() && endDateTime.getHours() < startDateTime.getHours()) {
    endDateTime.setDate(startDateTime.getDate() + 1);
  } else if ((startDateTime.getDate() > endDateTime.getDate() && startDateTime.getMonth() == endDateTime.getMonth() && startDateTime.getHours() < endDateTime.getHours()) || (startDateTime.getMonth() > endDateTime.getMonth())) {
    endDateTime.setDate(startDateTime.getDate());
    endDateTime.setMonth(startDateTime.getMonth());
  }
  
  var formattedEndDate = endDateTime.getFullYear() + "-" + padZero(endDateTime.getMonth() + 1) + "-" + padZero(endDateTime.getDate()) + "T" + padZero(endDateTime.getHours()) + ":" + padZero(endDateTime.getMinutes());

  endDateInput.value = formattedEndDate;

  localStorage.setItem("startDateStorage", startDate);
  localStorage.setItem("endDateStorage", formattedEndDate);

  var timeDiff = endDateTime.getTime() - startDateTime.getTime();
  var totalDays = Math.ceil(timeDiff / (1000 * 3600 * 24));

  localStorage.setItem("totalDays", totalDays);

  document.getElementById("totalDays").textContent = "x" + localStorage.getItem("totalDays") + " ngày";
  var totalPrice = unitPrice * totalDays;
  var formattedTotalPrice = totalPrice.toLocaleString();
  localStorage.setItem("totalPrice", formattedTotalPrice);
  document.getElementById("totalPrice").textContent = formattedTotalPrice + " VNĐ";
}

function padZero(value) {
  return value.toString().padStart(2, '0');
}




var closeButtonList = document.querySelectorAll('#close-toast');

closeButtonList.forEach(function(closeButton) {
  closeButton.addEventListener('click', function() {
    var toastElement = this.closest('.toast');

    if (toastElement) {
      toastElement.classList.add('hide');

      setTimeout(function() {
        toastElement.remove();
      }, 300);
    }
  });
});

document.addEventListener("click", function(event) {
  var collapseMenu = document.getElementById("collapseMenu");
  var targetElement = event.target;

  var isClickInside = collapseMenu.contains(targetElement);

  if (!isClickInside || targetElement.matches("#loginButton")) {
    collapseMenu.classList.remove("show");
  }
});
