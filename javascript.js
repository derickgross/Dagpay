const addEmployeeEndpoint = `https://dagpaypayroll.azurewebsites.net/api/AddEmployee`;
const addDependentEndpoint = `https://dagpaypayroll.azurewebsites.net/api/AddDependent`;
const getEmployeesAndDependentsEndpoint = `https://dagpaypayroll.azurewebsites.net/api/GetEmployeesAndDependents`;
let companyTotalDeduction = 0;

window.addEventListener('keydown', handleFirstTab);

function handleFirstTab(event) {
  if (event.key === 'Tab') {
    document.body.classList.add('tabNavigator');
    window.removeEventListener('keydown', handleFirstTab);
  }
}

document.addEventListener('DOMContentLoaded', function() {
  addEventListeners();
  populatePage();
});

function addToggleDependentsView() {
  const employees = document.getElementsByClassName('employee');

  for (employee of employees) {
    employee.addEventListener('click', function(event) {
      const dependentsList = event.target.closest('li.employee').querySelector('ul');

      if (!!dependentsList) {
        if (dependentsList.style.display === 'block') {
          dependentsList.style.display = 'none';
        } else {
          dependentsList.style.display = 'block';
        }
      }
    });

    employee.addEventListener('keydown', function(event) {
      const dependentsList = event.target.closest('li.employee').querySelector('ul');

      if (!!dependentsList && event.key === 'Enter') {
        if (dependentsList.style.display === 'block') {
          dependentsList.style.display = 'none';
        } else {
          dependentsList.style.display = 'block';
        }
      }
    });
  }
}

function populatePage() {
  document.getElementById('companyTotalDeduction').innerHTML = "(calculating...)";

  return fetch(getEmployeesAndDependentsEndpoint, { method: 'GET' })
  .then(function getDataCB(response) {
    if (response.status === 200) {
      return response.json()
      .then(function(data) {
        resetCompanyTotalDeduction();
        createBeneficiariesListItems(data);
        addToggleDependentsView();
      })
    } else {
      console.log('Response from GetEmployeesAndDependents endpoint was not OK');
    }
  })
  .catch(function getDataErrorCB(error) {
    console.log('An error occurred while retrieving beneficiaries: ' + error);
  })
}

function createBeneficiariesListItems(employees) {
  const ul = document.getElementById('beneficiariesList');
  ul.innerHTML = '';

  for (employee of employees) {
    let li = document.createElement('li');
    li.id = `employee-${employee.employeeid}`;
    li.className = 'employee'
    li.innerHTML = `${employee.lastname}, ${employee.firstname}: <span class="money">$${employee.deduction}</span>`;
    updateCompanyTotalDeduction(companyTotalDeduction, employee.deduction);

    if (!!employee.dependents) {
      let dependentsDeduction = 0;

      createDependentsListItems(employee.dependents, li, dependentsDeduction);
    }

    ul.append(li);

    let option = document.createElement('option');
    option.value = employee.employeeid;
    option.innerHTML = `${employee.lastname}, ${employee.firstname}`;
    document.getElementById('dependentEmployee').append(option);
  }
}

function setEmployeeOptions() {
  const select = document.getElementById('dependentEmployee');
  select.innerHTML = '<option value="" disabled selected>Select the dependent\'s employee</option>';

  let option = document.createElement('option');
  option.value = employee.employeeid;
  option.innerHTML = `${employee.lastname}, ${employee.firstname}`;
  select.append(option);
}

function createDependentsListItems(dependents, employeeLi, employeeDependentsDeduction) {
  let dependentUl = document.createElement('ul');

  for (dependent of dependents) {
    let dependentLi = document.createElement('li');
    updateCompanyTotalDeduction(dependent.deduction, companyTotalDeduction);
    dependentLi.innerHTML = `${dependent.lastname}, ${dependent.firstname}: <span class="money">$${dependent.deduction}</span>`;

    dependentUl.append(dependentLi);
    dependentUl.style.display = 'none';
    employeeDependentsDeduction = addMoneyValues(employeeDependentsDeduction, dependent.deduction);
  }

  employeeLi.setAttribute('tabindex', '0');
  employeeLi.innerHTML += ` (<span class="money dependents">+$${parseFloat(employeeDependentsDeduction).toFixed(2)}</span>)`;
  employeeLi.append(dependentUl);
}

function addMoneyValues(value1, value2) {
  return (parseFloat(value1 * 100) + parseFloat(value2 * 100))/100;
}

function updateCompanyTotalDeduction(deduction, total) {
  const totalElement = document.getElementById('companyTotalDeduction');

  companyTotalDeduction = addMoneyValues(total, deduction);

  totalElement.innerHTML = parseFloat(companyTotalDeduction).toFixed(2);
}

function isEmployeeFormValid() {
  const employeeId = document.getElementById('employeeId').value;
  const employeeFirstName = document.getElementById('employeeFirstname').value;
  const employeeLastName = document.getElementById('employeeLastname').value;

  return (fieldHasValue(employeeId) &&
      fieldHasValue(employeeFirstName) &&
      fieldHasValue(employeeLastName));
}

function isDependentFormValid() {
  const dependentEmployee = document.getElementById('dependentEmployee').value;
  const dependentFirstName = document.getElementById('dependentFirstname').value;
  const dependentLastName = document.getElementById('dependentLastname').value;

  return (fieldHasValue(dependentEmployee) &&
    fieldHasValue(dependentFirstName) &&
    fieldHasValue(dependentLastName));
}

function clearForms() {
  const inputs = document.getElementsByClassName('formInput');

  for (input of inputs) {
    input.value = '';
  }

  document.getElementById('dependentEmployee').value = '';
}

function fieldHasValue(field) {
  return (!!field);
}

function removeEmployeeIdNonNumericCharacters() {
  const employeeIdErrorMessage = document.getElementById('employeeIdErrorMessage');
  const string = document.getElementById('employeeId').value;
  const processedString = string.replace(/\D/g,'');

  employeeIdErrorMessage.style.display = 'none';
  if (string != processedString) {
    employeeIdErrorMessage.style.display = 'block';
    document.getElementById('employeeId').value = '';
  }
}

function addEmployeeCB(event) {
  event.preventDefault();
  const employeeFormErrorMessage = document.getElementById('employeeFormErrorMessage');
  employeeFormErrorMessage.style.display = 'none';

  if (isEmployeeFormValid()) {
    addEmployee();
  } else {
    employeeFormErrorMessage.style.display = 'block';
  }
}

function addDependentCB(event) {
  event.preventDefault();
  const dependentFormErrorMessage = document.getElementById('dependentFormErrorMessage');
  dependentFormErrorMessage.style.display = 'none';

  if (isDependentFormValid()) {
    addDependent();
  } else {
    dependentFormErrorMessage.style.display = 'block';
  }
}

function addEventListeners() {
  const addEmployeeButton = document.getElementById('employeeFormSubmit');
  const addDependentButton = document.getElementById('dependentFormSubmit');

  document.getElementById('employeeId').addEventListener('blur', removeEmployeeIdNonNumericCharacters);

  addEmployeeButton.addEventListener('click', addEmployeeCB);

  addDependentButton.addEventListener('click', addDependentCB);
}

function addEmployee() {
  const employee = {
    employeeid: document.getElementById('employeeId').value,
    firstname: document.getElementById('employeeFirstname').value,
    lastname: document.getElementById('employeeLastname').value
  }

  const body = `employeeid=${employee.employeeid}&firstname=${employee.firstname}&lastname=${employee.lastname}`;
  const errorElement = document.getElementById('employeeFormErrorMessage');

  fetch(addEmployeeEndpoint, { method: 'POST', body: body, headers: { 'Content-type': 'application/x-www-form-urlencoded' } })
  .then(function(response) {
    if (response.status === 200) {
      const confirmationElement = document.getElementById('confirmEmployeeAdded');
      displayConfirmationMessage(confirmationElement, employee);
      clearForms();
      populatePage();
    } else {
      displayErrorMessage(errorElement, employee);
    }
  })
  .catch(function(error) {
    displayErrorMessage(errorElement, employee);
  });
}

function addDependent() {
  const dependent = {
    employeeid: document.getElementById('dependentEmployee').value,
    firstname: document.getElementById('dependentFirstname').value,
    lastname: document.getElementById('dependentLastname').value
  }

  const body = `employeeid=${dependent.employeeid}&firstname=${dependent.firstname}&lastname=${dependent.lastname}`;
  const errorElement = document.getElementById('dependentFormErrorMessage');

  fetch(addDependentEndpoint, { method: 'POST', body: body, headers: { 'Content-type': 'application/x-www-form-urlencoded' } })
  .then(function(response) {
    if (response.status === 200) {
      const confirmationElement = document.getElementById('confirmDependentAdded');
      displayConfirmationMessage(confirmationElement, dependent);
      clearForms();
      populatePage();
    } else {
      displayErrorMessage(errorElement, dependent);
    }
  })
  .catch(function(error) {
    displayErrorMessage(errorElement, dependent);
  });
}

function displayConfirmationMessage(element, beneficiary) {
  element.style.display = 'block';
  element.innerHTML = `${beneficiary.firstname} ${beneficiary.lastname} was successfully added.`;
}

function displayErrorMessage(element, beneficiary) {
  element.style.display = 'block';
  element.innerHTML = `Unable to add ${beneficiary.firstname} ${beneficiary.lastname}. Please try again.`;
}

function resetCompanyTotalDeduction() {
  companyTotalDeduction = 0;
}
