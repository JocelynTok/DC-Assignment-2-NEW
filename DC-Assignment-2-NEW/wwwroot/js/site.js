

// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// ----------------------------------------------------------Loading Different View -----------------------------------------------------------------------
function loadView(status) {
    var apiUrl = '/api/login/defaultview';
    fetchAccountDetails();
    if (status === "authview") {
        apiUrl = '/api/login/authview';
    } else if (status === "error") {
        apiUrl = '/api/login/error';
    } else if (status === "about") {
        apiUrl = '/api/about/view';
    } else if (status === "logout") {
        apiUrl = '/api/logout';
    } 

    console.log("Hello " + apiUrl);

    fetch(apiUrl)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.text();
        })
        .then(data => {
            // Handle the data from the API
            document.getElementById('main').innerHTML = data;
            if (status === "logout") {
                document.getElementById('LogoutButton').style.display = "none";
                document.getElementById('LoginButton').style.display = "block";
            }
            else if (status != "logout"){
                document.getElementById('LoginButton').style.display = "none";
                document.getElementById('LogoutButton').style.display = "block";

            }
            
        })
        .catch(error => {
            // Handle any errors that occurred during the fetch
            console.error('Fetch error:', error);
        });

}

// ---------------------------------------------------------- Loading Admin View  -----------------------------------------------------------------------
function loadAdminView(status) {
    var apiUrl = '/api/login/authview';
    fetchAccountDetails();
    if (status === "admin-profile") {
        apiUrl = '/api/admin/profile';
        fetchAdminProfileDetails();
    } else if (status === "transaction-management") {
        apiUrl = '/api/admin/transactionManagement';
        loadAllTransactions();
    } else if (status === "user-management") {
        apiUrl = '/api/admin/userManagement';
        loadAllUsers();
    } else if (status === "admin-management") {
        apiUrl = '/api/admin/adminManagement';
        loadAllAdmins();
    }

    console.log("Hello " + apiUrl);

    fetch(apiUrl)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.text();
        })
        .then(data => {
            // Handle the data from the API
            document.getElementById('content').innerHTML = data;
            document.getElementById('menu').style.display = 'none';
       
        })
        .catch(error => {
            // Handle any errors that occurred during the fetch
            console.error('Fetch error:', error);
        });

}

// ---------------------------------------------------------- Loading User View  -----------------------------------------------------------------------
function loadUserView(status) {
    var apiUrl = '/api/login/authview';
    fetchAccountDetails();
    if (status === "user-profile") {
        apiUrl = '/api/useraction/profile';
        fetchUserProfileDetails();
    } else if (status === "transaction-history") {
        apiUrl = '/api/useraction/history';
        loadTransactions();
    }

    console.log("Hello " + apiUrl);

    fetch(apiUrl)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.text();
        })
        .then(data => {
            // Handle the data from the API
            document.getElementById('content').innerHTML = data;
            document.getElementById('summary').style.display = 'none';

        })
        .catch(error => {
            // Handle any errors that occurred during the fetch
            console.error('Fetch error:', error);
        });

}

// ---------------------------------------------------------- Login Validation -----------------------------------------------------------------------
function performAuth() {

    var email = document.getElementById('Email').value;
    var password = document.getElementById('Password').value;
    var data = {
        Email: email,
        PasswordHash: password
    };
    console.error(data);
    const apiUrl = '/api/login/auth';

    const headers = {
        'Content-Type': 'application/json', 

    };

    const requestOptions = {
        method: 'POST',
        headers: headers,
        body: JSON.stringify(data) // Convert the data object to a JSON string
    };

    fetch(apiUrl, requestOptions)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            // Handle the data from the API
            const jsonObject = data;
            if (jsonObject.login) {
                loadView("authview");
                document.cookie = "userEmail=" + email;
                if (document.cookie) {
                    const cookies = document.cookie.split('; ');
                    for (let i = 0; i < cookies.length; i++) {
                        const cookie = cookies[i].split('=');
                        if (cookie[0] === 'sessionID') {
                            const sessionID = decodeURIComponent(cookie[1]);
                            if (sessionID === "123456789") {
                                fetchAdminProfileDetails();
                            } else if (sessionID === "223456789") {
                                
                                fetchUserProfileDetails();
                               
                               
                            }

                            break;
                        }
                    }
                }
                fetchAccountDetails(); 
                document.getElementById('LogoutButton').style.display = "block";
            
            }
            else {
                loadView("error");
            }

        })
        .catch(error => {
            // Handle any errors that occurred during the fetch
            console.error('Fetch error:', error);
        });



}

// ---------------------------------------------------------- User Account Summary  -----------------------------------------------------------------------
function fetchAccountDetails() {
    const email = document.cookie.replace(/(?:(?:^|.*;\s*)userEmail\s*=\s*([^;]*).*$)|^.*$/, "$1");
    const userProfileApiUrl = `/api/userProfile/${email}`;

    
    fetch(userProfileApiUrl)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(userProfileData => {
            console.log(userProfileData);
           
            const accountNo = userProfileData.accountNo;
                const accountApiUrl = `/api/account/${accountNo}`;

                fetch(accountApiUrl)
                    .then(response => {
                        if (!response.ok) {
                            throw new Error('Network response was not ok');
                        }
                        return response.json();
                    })
                    .then(accountData => {
                        if (accountNo && accountData.balance) {
                            document.getElementById('acctNo').textContent = accountNo;
                            document.getElementById('balance').textContent = accountData.balance;

                   
                        }

                    })
                    .catch(error => {
                        console.error('Fetch error:', error);

                    });
            
        })
        .catch(error => {
            console.error('Fetch error:', error);

        });
}

// ---------------------------------------------------------- User Profile -----------------------------------------------------------------------
function fetchUserProfileDetails() {
    const email = document.cookie.replace(/(?:(?:^|.*;\s*)userEmail\s*=\s*([^;]*).*$)|^.*$/, "$1");
    const userProfileApiUrl = `/api/userProfile/${email}`;


    fetch(userProfileApiUrl)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(userProfileData => {
       //     console.log(userProfileData);
            document.getElementById('profileImage').src = userProfileData.pictureUrl;
            document.getElementById('acctNo').textContent =  userProfileData.accountNo;
            document.getElementById('username').textContent =  userProfileData.username;
            document.getElementById('email').textContent = userProfileData.email;
            document.getElementById('address').textContent = userProfileData.address;
            document.getElementById('phone').textContent = userProfileData.phone;


     
        })
        .catch(error => {
            console.error('Fetch error:', error);

        });
}


// ---------------------------------------------------------- User Loading Transaction -----------------------------------------------------------------------
function loadTransactions() {
    // Step 1: Retrieve userEmail from cookies
    const userEmail = document.cookie.replace(/(?:(?:^|.*;\s*)userEmail\s*=\s*([^;]*).*$)|^.*$/, "$1");

    if (userEmail) {
        // Step 2: Use userEmail to fetch accountNo from userProfile
        fetch(`/api/userProfile/${userEmail}`)
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json();
            })
            .then(userProfileData => {
                const accountNo = userProfileData.accountNo;

                // Step 3: Use accountNo to fetch transactions
                fetch(`/api/transaction/account/${accountNo}`)
                    .then(response => {
                        if (!response.ok) {
                            throw new Error('Network response was not ok');
                        }
                        return response.json();
                    })
                    .then(transactions => {
                        // Step 4: Display the transactions in a table
                        const tableBody = document.getElementById('transactionTableBody');
                        tableBody.innerHTML = ''; // Clear existing rows
                        console.log(transactions);

                        transactions.forEach(transaction => {
                            console.log(transaction);
                            const row = tableBody.insertRow();
                            
                            row.insertCell(0).textContent = transaction.transactionID;
                            row.insertCell(1).textContent = transaction.transactionType;
                            row.insertCell(2).textContent = transaction.accountNo;
                            row.insertCell(3).textContent = transaction.amount;
                            row.insertCell(4).textContent = transaction.transactionDate;
                            row.insertCell(5).textContent = transaction.description;

                        });
                    })
                    .catch(error => {
                        console.error('Fetch error:', error);
                    });
            })
            .catch(error => {
                console.error('Fetch error:', error);
            });
    } else {
        console.error('UserEmail not found in cookies');
    }

}

// --------------------------------------------------------- User Create Transaction ----------------------------------
function loadCreateTransaction() {
    const modal = document.getElementById("create-transaction");
    modal.style.display = "block";

    const confirmTransaction = document.getElementById("confirm-transaction");
    const cancelTransaction = document.getElementById("cancel-transaction");

    confirmTransaction.addEventListener('click', () => {
        let dropdown = document.getElementById("transaction-type");
        let selectedIndex = dropdown.selectedIndex;
        const transactionType = dropdown.options[selectedIndex].text;
        //get transaciton type from form
        const amount = document.getElementById("amount").value;
        //console.log(transactionType + document.getElementById("amount").value);

        const accountID = document.getElementById("acctNo").text;
        console.log(accountID);
        //create new transaction
        addNewTransaction(accountID, transactionType, amount);
        
        modal.style.display = 'none';
        dropdown.selectedIndex = 0;
        document.getElementById("amount").value = '';


    });

    cancelTransaction.addEventListener('click', () => {
        let dropdown = document.getElementById("transaction-type");
        let selectedIndex = dropdown.selectedIndex;
        modal.style.display = 'none';
        dropdown.selectedIndex = 0;
        document.getElementById("amount").value = '';
    });
}


// Function to update the user profile and account  (admin/user)
function addNewTransaction(accountID, transactionType, amount) {

    /*
    // Fetch the existing user profile based on the old email
    fetch(`/api/transaction/`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    })
        .then(response => {
            if (response.ok) {
                return response.json();
            } else {
                console.error('Error accessing transaction page');
                throw new Error('Failed to access transaction api');
            }
        })
        .then((json) => console.log(json))
        .catch(error => {
            console.error('Error creating new transaction:', error);
        });*/
    //string accountNo = accountID.toString();
    
        //new transaction
    const createdTransaction = {
        transactionID: "36",
        transactionType: transactionType,
        amount: amount,
        accountNo: "123555",
        transactionDate: "10/15/2023 20:01:48",
        description: ""

    };

    // Send a POST request to create the user
    fetch('/api/transaction', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(createdTransaction),
    })
        .then(response => {
            if (response.ok) {
                console.log('Transaction inserted successfully');
            

            } else {
                console.error('Error creating transaction');
            }
        })

}

// ---------------------------------------------------------- Admin Profile-----------------------------------------------------------------------
function fetchAdminProfileDetails() {
    const email = document.cookie.replace(/(?:(?:^|.*;\s*)userEmail\s*=\s*([^;]*).*$)|^.*$/, "$1");
    const userProfileApiUrl = `/api/userProfile/${email}`;


    fetch(userProfileApiUrl)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(userProfileData => {
            console.log(userProfileData);
            document.getElementById('AprofileImage').src = userProfileData.pictureUrl;
            document.getElementById('AacctNo').textContent = userProfileData.accountNo;
            document.getElementById('Ausername').textContent = userProfileData.username;
            document.getElementById('Aemail').textContent = userProfileData.email;
            document.getElementById('Aaddress').textContent = userProfileData.address;
            document.getElementById('Aphone').textContent = userProfileData.phone;

        })
        .catch(error => {
            console.error('Fetch error:', error);

        });
}

// ---------------------------------------------------------- Handling admin and user update profile-----------------------------------------------------------------------
function adminUpdateProfile() {
    const modal = document.getElementById("update-modal");
    modal.style.display = "block";

    const accountNo = document.getElementById("AacctNo").textContent;
    const oldEmail = document.getElementById("Aemail").textContent;
    const confirmButton = document.getElementById("confirm-update");
    const cancelButton = document.getElementById("cancel-update");
    console.log(accountNo, oldEmail);

    confirmButton.addEventListener('click', () => {
        const newEmail = document.getElementById("new-email").value;
        const newPassword = document.getElementById("new-password").value;
        const newAddress = document.getElementById("new-address").value;
        const newPhone = document.getElementById("new-phone").value;

        // Call the function to update the user profile and account
        updateUserProfileAndAccount(oldEmail, newEmail, newPassword, newAddress, newPhone, accountNo);

        modal.style.display = 'none';
        document.getElementById("new-email").value = "";
        document.getElementById("new-address").value = "";
        document.getElementById("new-phone").value = "";
        document.getElementById("new-password").value = "";

    });

        cancelButton.addEventListener('click', () => {
            modal.style.display = 'none';
            document.getElementById("new-email").value = "";
            document.getElementById("new-address").value = "";
            document.getElementById("new-phone").value = "";
            document.getElementById("new-password").value = "";
        });
    
}

function userUpdateProfile() {
    const modal = document.getElementById("update-modal");
    modal.style.display = "block";

    const accountNo = document.getElementById("acctNo").textContent;
    const oldEmail = document.getElementById("email").textContent;
    const confirmButton = document.getElementById("confirm-update");
    const cancelButton = document.getElementById("cancel-update");
    console.log(accountNo, oldEmail);

    confirmButton.addEventListener('click', () => {
        const newEmail = document.getElementById("new-email").value;
        const newPassword = document.getElementById("new-password").value;
        const newAddress = document.getElementById("new-address").value;
        const newPhone = document.getElementById("new-phone").value;

        // Call the function to update the user profile and account
        updateUserProfileAndAccount(oldEmail, newEmail, newPassword, newAddress, newPhone, accountNo);

        modal.style.display = 'none';
        document.getElementById("new-email").value = "";
        document.getElementById("new-address").value = "";
        document.getElementById("new-phone").value = "";
        document.getElementById("new-password").value = "";

    });

    cancelButton.addEventListener('click', () => {
        modal.style.display = 'none';
        document.getElementById("new-email").value = "";
        document.getElementById("new-address").value = "";
        document.getElementById("new-phone").value = "";
        document.getElementById("new-password").value = "";
    });

}


// Function to update the user profile and account  (admin/user)
function updateUserProfileAndAccount(oldEmail, newEmail, newPassword, newAddress, newPhone, accountNo) {

    // Fetch the existing user profile based on the old email
    fetch(`/api/userProfile/${oldEmail}`)
        .then(response => {
            if (response.ok) {
                return response.json();
            } else {
                console.error('Error fetching existing user profile');
                throw new Error('Profile not found');
            }
        })
        .then(existingUserProfile => {
            // Create an object with the updated data
            const updatedUserProfile = {
                username: existingUserProfile.username,
                email: newEmail,
                address: newAddress,
                phone: newPhone,
                pictureUrl: existingUserProfile.pictureUrl,
                passwordHash: newPassword,
                accountNo: accountNo,
                roles: existingUserProfile.roles
            };

            // Send an HTTP PUT request to update the user profile
            return fetch(`/api/userProfile`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(updatedUserProfile),
            });
        })
        .then(response => {
            if (response.ok) {
                console.log('User profile updated successfully');
                updateAccount(newEmail, accountNo);
                document.cookie = 'userEmail' + '=' + newEmail;
                fetchAdminProfileDetails();
                fetchUserProfileDetails();

            } else {
                console.error('Error updating user profile');
            }
        })
        .catch(error => {
            console.error('Error updating user profile:', error);
        });
}
// ---------------------------------------------------------- Admin admin list view -----------------------------------------------------------------------
function loadAllAdmins() {
    fetch(`/api/userProfile`)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(userProfileData => {
            const tableBody = document.getElementById('admin-table-body');
            tableBody.innerHTML = ''; // Clear existing rows
            console.log(userProfileData);

            userProfileData.forEach(data => {
                console.log(data);

                if (data.roles !== 'user') {
                    const row = tableBody.insertRow();
                    row.insertCell(0).textContent = data.username;
                    row.insertCell(1).textContent = data.email;
                    row.insertCell(2).textContent = data.passwordHash;
                    row.insertCell(3).textContent = data.address;
                    row.insertCell(4).textContent = data.phone;
                    row.insertCell(5).textContent = data.accountNo;

                    // Cell 6: Display balance (fetched from API)
                    const cell6 = row.insertCell(6);
                    cell6.id = 'balance-cell-' + data.accountNo; // Assign an ID for easy reference

                    // Fetch balance based on accountNo
                    fetch(`/api/account/${data.accountNo}`)
                        .then(response => {
                            if (!response.ok) {
                                throw new Error('Network response was not ok');
                            }
                            return response.json();
                        })
                        .then(accountData => {
                            // Assuming accountData contains balance
                            const balanceCell = document.getElementById('balance-cell-' + data.accountNo);
                            balanceCell.textContent = accountData.balance;
                        })
                        .catch(error => {
                            console.error('Fetch error:', error);
                        });
                }
            });
        })
        .catch(error => {
            console.error('Fetch error:', error);
        });
}

//Hanlding user management search 
function handleAdminSearch() {
    const searchTerm = document.getElementById("search-bar").value.trim().toLowerCase();
    const rows = document.getElementById("admin-table-body").getElementsByTagName('tr');

    for (let i = 0; i < rows.length; i++) {
        const email = rows[i].getElementsByTagName('td')[1].textContent.trim().toLowerCase();
        if (email.includes(searchTerm)) {
            rows[i].style.display = '';
        } else {
            rows[i].style.display = 'none';
        }
    }
}

//clear user search
function clearAdminSearch() {
    document.getElementById("search-bar").value = "";
    const rows = document.getElementById("admin-table-body").getElementsByTagName('tr');

    for (let i = 0; i < rows.length; i++) {
        rows[i].style.display = '';
    }
}

// ---------------------------------------------------------- Admin user management view -----------------------------------------------------------------------
    function loadAllUsers() {
        fetch(`/api/userProfile`)
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json();
            })
            .then(userProfileData => {
                const tableBody = document.getElementById('user-table-body');
                tableBody.innerHTML = ''; // Clear existing rows
                console.log(userProfileData);

                userProfileData.forEach(data => {
                    console.log(data);

                    if (data.roles !== 'admin') {
                        const row = tableBody.insertRow();
                        row.insertCell(0).textContent = data.username;
                        row.insertCell(1).textContent = data.email;
                        row.insertCell(2).textContent = data.passwordHash;
                        row.insertCell(3).textContent = data.address;
                        row.insertCell(4).textContent = data.phone;
                        row.insertCell(5).textContent = data.accountNo;

                        // Cell 6: Display balance (fetched from API)
                        const cell6 = row.insertCell(6);
                        cell6.id = 'balance-cell-' + data.accountNo; // Assign an ID for easy reference

                        // Cell 7: Edit and Delete buttons
                        const cell7 = row.insertCell(7);

                        // Edit button
                        const editButton = document.createElement('button');
                        editButton.textContent = 'Edit';
                        editButton.addEventListener('click', () => openEditModal(data.accountNo));
                        cell7.appendChild(editButton);

                        // Delete button
                        const deleteButton = document.createElement('button');
                        deleteButton.textContent = 'Delete';
                        deleteButton.addEventListener('click', () => handleDelete(data.username, data.email));
                        cell7.appendChild(deleteButton);

                        // Fetch balance based on accountNo
                        fetch(`/api/account/${data.accountNo}`)
                            .then(response => {
                                if (!response.ok) {
                                    throw new Error('Network response was not ok');
                                }
                                return response.json();
                            })
                            .then(accountData => {
                                // Assuming accountData contains balance
                                const balanceCell = document.getElementById('balance-cell-' + data.accountNo);
                                balanceCell.textContent = accountData.balance;
                            })
                            .catch(error => {
                                console.error('Fetch error:', error);
                            });
                    }
                });
            })
            .catch(error => {
                console.error('Fetch error:', error);
            });
    }

    //Hanlding user management search 
    function handleUserSearch() {
        const searchTerm = document.getElementById("search-bar").value.trim().toLowerCase();
        const rows = document.getElementById("user-table-body").getElementsByTagName('tr');

        for (let i = 0; i < rows.length; i++) {
            const email = rows[i].getElementsByTagName('td')[1].textContent.trim().toLowerCase();
            if (email.includes(searchTerm)) {
                rows[i].style.display = '';
            } else {
                rows[i].style.display = 'none';
            }
        }
    }

    //clear user search
    function clearUserSearch() {
        document.getElementById("search-bar").value = "";
        const rows = document.getElementById("user-table-body").getElementsByTagName('tr');

        for (let i = 0; i < rows.length; i++) {
            rows[i].style.display = '';
        }
    }
    //Handling create user and user modal
    function createUser() {
        const modal = document.getElementById("createUserModal");
        modal.style.display = "block";

        const cancelButton = document.getElementById("cancel-create");

        cancelButton.addEventListener('click', () => {
            modal.style.display = 'none';

            document.getElementById('username').value = "";
            document.getElementById('email').value = "";
            document.getElementById('address').value = "";
            document.getElementById('phone').value = "";
            document.getElementById('pictureUrl').value = "";
            document.getElementById('password').value = "";
        });

        document.getElementById('createUserForm').addEventListener('submit', function (event) {
            event.preventDefault();

            // Collect form data
            const newUsername = document.getElementById('username').value;
            const newEmail = document.getElementById('email').value;
            const newAddress = document.getElementById('address').value;
            const newPhone = document.getElementById('phone').value;
            const newPictureUrl = document.getElementById('pictureUrl').value;
            const newPassword = document.getElementById('password').value;
            const newAcctNo = generateAccountNumber();

            // Create a new user object
            const newUser = {
                username: newUsername,
                email: newEmail,
                address: newAddress,
                phone: newPhone,
                pictureUrl: newPictureUrl,
                passwordHash: newPassword,
                accountNo: newAcctNo,
                roles: 'user', // Set the default role to 'user'
            };

            const newAccount = {
                accountNo: newAcctNo,
                username: newUsername,
                email: newEmail,
                balance: 0
            }

            // Send a POST request to create the user
            fetch('/api/userProfile', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(newUser),
            })
                .then(response => {
                    if (response.ok) {
                        console.log('User created successfully');

                        fetch('/api/account', {
                            method: 'POST',
                            headers: {
                                'Content-Type': 'application/json',
                            },
                            body: JSON.stringify(newAccount),
                        })
                            .then(response => {
                                if (response.ok) {
                                    console.log('Account created successfully');

                                    // Logging activity 
                                    const newUser = {
                                        Username: newUsername,
                                    };

                                    // Define the API endpoint URL
                                    const apiUrl = '/api/admin/admincreateUser';

                                    const requestOptions = {
                                        method: 'POST',
                                        headers: {
                                            'Content-Type': 'application/json', // Specify the content type as JSON
                                        },
                                        body: JSON.stringify(newUser), 
                                    };

                                    // Send the POST request to create the user
                                    fetch(apiUrl, requestOptions)
                                        .then(response => {
                                            if (response.ok) {
                                                console.log('Create Activity sucessfully logged');
                                            } else {
                                                console.error('Error logging create activity');
                                            }
                                        })
                                        .catch(error => {
                                            console.error('Create user request error:', error);
                                        });
                                } else {
                                    console.error('Error creating account');
                                }
                            })
                            .catch(error => {
                                console.error('Create account request error:', error);
                            });

                        modal.style.display = 'none';

                        document.getElementById('username').value = "";
                        document.getElementById('email').value = "";
                        document.getElementById('address').value = "";
                        document.getElementById('phone').value = "";
                        document.getElementById('pictureUrl').value = "";
                        document.getElementById('password').value = "";

                        loadAllUsers();
                    } else {
                        console.error('Error creating user');
                    }
                })
                .catch(error => {
                    console.error('Create user request error:', error);
                });
        });

}
    //generate random account number
    function generateAccountNumber() {
        const min = 100000000; // Smallest 9-digit number
        const max = 999999999; // Largest 9-digit number

        // Generate a random number within the specified range
        const randomNumber = Math.floor(Math.random() * (max - min + 1)) + min;

        // Convert the random number to a string
        const accountNumber = randomNumber.toString();

        return accountNumber;
    }

    //Handling edit and edit modal
    function openEditModal(accountNo) {
        const modal = document.getElementById("edit-modal");
        modal.style.display = "block";
        console.log('Edit clicked for accountNo:', accountNo);

        // Capture the oldEmail when the "Edit" button is clicked
        const oldEmailToEdit = getEmailFromRow(accountNo);
        const confirmButton = document.getElementById("confirm");
        const cancelButton = document.getElementById("cancel");
        console.log(oldEmailToEdit);
        confirmButton.onclick = function () {
            // Call confirmAction with the oldEmail parameter
            confirmAction(oldEmailToEdit, accountNo);

            modal.style.display = 'none';

            document.getElementById("new-email").value = "";
            document.getElementById("new-password").value = "";
            document.getElementById("new-address").value = "";
            document.getElementById("new-phone").value = "";
        };

        cancelButton.addEventListener('click', () => {
            modal.style.display = 'none';


            document.getElementById("new-email").value = "";
            document.getElementById("new-password").value = "";
            document.getElementById("new-address").value = "";
            document.getElementById("new-phone").value = "";
        });

    }

    function getEmailFromRow(accountNo) {
        // Find the row that matches the given accountNo
        const rows = document.getElementById("user-table-body").getElementsByTagName('tr');
        for (let i = 0; i < rows.length; i++) {
            const accountCell = rows[i].getElementsByTagName('td')[5].textContent;
            if (accountCell.trim() === accountNo) {
                const email = rows[i].getElementsByTagName('td')[1].textContent;
                return email;
            }
        }
        return null; // Return null if not found
    }

    function confirmAction(oldEmail, AccountNo) {
        // Use the captured oldEmail
        const newEmail = document.getElementById("new-email").value;
        const newPassword = document.getElementById("new-password").value;
        const newAddress = document.getElementById("new-address").value;
        const newPhone = document.getElementById("new-phone").value;

        // Fetch the existing user profile based on the old email
        fetch(`/api/userProfile/${oldEmail}`)
            .then(response => {
                if (response.ok) {
                    return response.json();
                } else {
                    console.error('Error fetching existing user profile');
                    throw new Error('Profile not found');
                }
            })
            .then(existingUserProfile => {
                // Create an object with the updated data
                const updatedUserProfile = {
                    username: existingUserProfile.username,
                    email: newEmail,
                    address: newAddress,
                    phone: newPhone,
                    pictureUrl: existingUserProfile.pictureUrl,
                    passwordHash: newPassword,
                    accountNo: AccountNo,
                    roles: existingUserProfile.roles
                };

                // Send an HTTP PUT request to update the user profile
                return fetch(`/api/userProfile`, {
                    method: 'PUT',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify(updatedUserProfile),
                });
            })
            .then(response => {
                if (response.ok) {
                    console.log('User profile updated successfully');
                    updateAccount(newEmail, AccountNo);
                    loadAllUsers();

                } else {
                    console.error('Error updating user profile');
                }
            })
            .catch(error => {
                console.error('Error updating user profile:', error);
            });

    }

    //update account when userProfile was modified
    function updateAccount(newEmail, AccountNo) {

        let username_st = "";
        // Fetch the existing user profile based on the old email
        fetch(`/api/account/${AccountNo}`)
            .then(response => {
                if (response.ok) {
                    return response.json();
                } else {
                    console.error('Error fetching existing account ');
                    throw new Error('Account not found');
                }
            })
            .then(accountData => {
                console.log(accountData);
                // Create an object with the updated data
                const updatedAccount = {
                    accountNo: AccountNo,
                    username: accountData.username,
                    email: newEmail,
                    balance: accountData.balance
                };

                username_st = accountData.username;

                // Send an HTTP PUT request to update the user profile
                return fetch(`/api/account`, {
                    method: 'PUT',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify(updatedAccount),
                });
            })
            .then(response => {
                if (response.ok) {
                    console.log('Account updated successfully');
                    loadAllUsers();

                    // Logging activity 
                    const updatedUser = {
                        Username: username_st,
                    };

                    const apiUrl = '/api/admin/adminupdateUser';

                    const requestOptions = {
                        method: 'POST',  
                        headers: {
                            'Content-Type': 'application/json',  // Specify the content type as JSON
 
                        },
                        body: JSON.stringify(updatedUser),  
                    };

                    // Send the POST request to update the user
                    fetch(apiUrl, requestOptions)
                        .then(response => {
                            if (response.ok) {
                                console.log('Update Activity sucessfully logged');
                            } else {
                                console.error('Error logging update activity');
                            }
                        })
                        .catch(error => {
                            console.error('Update user request error:', error);
                        });
                } else {
                    console.error('Error updating account');
                }
            })
            .catch(error => {
                console.error('Error updating account:', error);
            });
    }

    //handling delete user and delete modal

    function handleDelete(username, email) {
        // Show the delete user modal
        const modal = document.getElementById('delete-modal');
        modal.style.display = 'block';

        // Add event listeners to the confirm and cancel buttons
        const confirmButton = document.getElementById('confirmDelete');
        const cancelButton = document.getElementById('cancelDelete');

        confirmButton.addEventListener('click', () => {
            deleteUser(username, email);

            modal.style.display = 'none';
        });

        cancelButton.addEventListener('click', () => {
            modal.style.display = 'none';
        });

        document.getElementById('deleteUsername').textContent = username;
        document.getElementById('deleteEmail').textContent = email;
    }


    function deleteUser(username, email) {
        // First, fetch the user's account number from their profile
        fetch(`/api/userProfile/${email}`)
            .then(response => {
                if (response.ok) {
                    return response.json();
                } else {
                    throw new Error(`Error fetching user profile for ${username}`);
                }
            })
            .then(userData => {
                const accountNo = userData.accountNo;

                // Now, delete the user's profile
                const profileUrl = `/api/userProfile/${username}`;
                const profileRequestOptions = {
                    method: 'DELETE',
                };

                fetch(profileUrl, profileRequestOptions)
                    .then(response => {
                        if (response.ok) {
                            console.log(`User with username ${username} successfully deleted.`);

                            // Now, delete the user's account
                            const accountUrl = `/api/account/${accountNo}`;
                            const accountRequestOptions = {
                                method: 'DELETE',
                            };

                            fetch(accountUrl, accountRequestOptions)
                                .then(accountResponse => {
                                    if (accountResponse.ok) {
                                        console.log(`Account associated with ${username} (${accountNo}) successfully deleted.`);
                                        deleteTransactionsByAccount(accountNo)

                                        // Logging activity 
                                        const apiUrl = `/api/admin/admindeleteUser?username=${username}`;

                                        const requestOptions = {
                                            method: 'POST', 
                                            headers: {
                                                'Content-Type': 'application/json',  // Specify the content type as JSON
                                            },
                                          
                                        };

                                        // Send the POST request to delete the user
                                        fetch(apiUrl, requestOptions)
                                            .then(response => {
                                                if (response.ok) {
                                                    console.log('Delete Activity sucessfully logged');
                                                } else {
                                                    console.error('Error logging delete activity');
                                                }
                                            })
                                            .catch(error => {
                                                console.error('Delete user request error:', error);
                                            });
                                    } else {
                                        console.error(`Error deleting account for ${username} (${accountNo}).`);
                                    }
                                })
                                .catch(accountError => {
                                    console.error('Error deleting user account:', accountError);
                                });

                            // Optionally, you can load updated user data here if needed
                            loadAllUsers();
                        } else {
                            console.error(`Error deleting user with username ${username}.`);
                        }
                    })
                    .catch(profileError => {
                        console.error('Error deleting user profile:', profileError);
                    });
            })
            .catch(userError => {
                console.error('Error fetching user profile:', userError);
            });
    }

function deleteTransactionsByAccount(accountNo) {
    // Fetch transactions by accountNo
    console.log(accountNo);
    fetch(`/api/transaction/account/${accountNo}`)
        .then(response => {
            if (response.ok) {
                return response.json();
            } else {
                console.error('Error fetching transactions for accountNo:', accountNo);
                throw new Error('Transactions not found');
            }
        })
        .then(transactions => {
            console.log('Transactions:', transactions);
            // Loop through the transactions and delete each one
            transactions.forEach(transaction => {
                console.log(transaction);
                deleteTransaction(transaction.transactionID);
            });
        })
        .catch(error => {
            console.error('Error fetching transactions:', error);
        });
}

function deleteTransaction(transactionID) {
    // Send a DELETE request to delete the transaction by transactionID
    fetch(`/api/transaction/${transactionID}`, {
        method: 'DELETE',
    })
        .then(response => {
            if (response.ok) {
                console.log(`Transaction with ID ${transactionID} successfully deleted.`);
            } else {
                console.error(`Error deleting transaction with ID ${transactionID}.`);
            }
        })
        .catch(error => {
            console.error(`Error deleting transaction with ID ${transactionID}:`, error);
        });
}

// ---------------------------------------------------------- Admin transaction management view -----------------------------------------------------------------------
    function loadAllTransactions() {
        fetch(`/api/transaction`)
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json();
            })
            .then(transactions => {
                const tableBody = document.getElementById('transaction-table-body');
                tableBody.innerHTML = ''; // Clear existing rows
                console.log(transactions);

                transactions.forEach(transaction => {
                    console.log(transaction);
                    const row = tableBody.insertRow();

                    row.insertCell(0).textContent = transaction.transactionID;
                    row.insertCell(1).textContent = transaction.transactionType;
                    row.insertCell(2).textContent = transaction.accountNo;
                    row.insertCell(3).textContent = transaction.amount;
                    row.insertCell(4).textContent = transaction.transactionDate;
                    row.insertCell(5).textContent = transaction.description;
                });
            })
            .catch(error => {
                console.error('Fetch error:', error);
            });

    }

    //handle transaction search by accountNo
    function handleSearch() {
        const searchTerm = document.getElementById("search-bar").value.trim().toLowerCase();
        const rows = document.getElementById("transaction-table-body").getElementsByTagName('tr');

        for (let i = 0; i < rows.length; i++) {
            const accountNo = rows[i].getElementsByTagName('td')[2].textContent.trim().toLowerCase();
            if (accountNo.localeCompare(searchTerm) === 0) {
                rows[i].style.display = '';
            } else {
                rows[i].style.display = 'none';
            }
        }
}

    //handle transaction search by date range
    function handleDateSearch() {
        const startDate = new Date(document.getElementById("start-date-bar").value + ", 00:00:00");
        //the .value is important to get the value of the date inputted
        const endDate = new Date(document.getElementById("end-date-bar").value + ", 00:00:00");
        //console.log(startDate + " " +endDate);
        const rows = document.getElementById("transaction-table-body").getElementsByTagName('tr');

        
        for (let i = 0; i < rows.length; i++) {
            const tempDate = new Date(rows[i].getElementsByTagName('td')[4].textContent);
            if ((tempDate >= startDate) && (tempDate <= endDate)) {
                // do >= <= instead of only === bc we have DateTime instead of just Date
                rows[i].style.display = '';
            } else {
                rows[i].style.display = 'none';
            }
        }
        
    }

    //handling filter by trasnaction type
    function handleFilter(transactionType) {
        const rows = document.getElementById("transaction-table-body").getElementsByTagName('tr');

        for (let i = 0; i < rows.length; i++) {
            const type = rows[i].getElementsByTagName('td')[1].textContent.trim(); // Assuming transaction type is in the second column
            if (type === transactionType) {
                rows[i].style.display = '';
            } else {
                rows[i].style.display = 'none';
            }
        }
    }

    //handling sort ascending / descending for amount
    function handleSort(ascending) {
        const rows = Array.from(document.getElementById("transaction-table-body").getElementsByTagName('tr'));
        const sortFactor = ascending ? 1 : -1;

        rows.sort((a, b) => {
            const amountA = parseFloat(a.getElementsByTagName('td')[3].textContent);
            const amountB = parseFloat(b.getElementsByTagName('td')[3].textContent);
            return sortFactor * (amountA - amountB);
        });

        const tbody = document.getElementById("transaction-table-body");
        rows.forEach(row => tbody.appendChild(row));
}

    //handling date sort ascending / descending
    function handleDateSort(ascending) {
        const rows = Array.from(document.getElementById("transaction-table-body").getElementsByTagName('tr'));
        const sortFactor = ascending ? 1 : -1;

        rows.sort((a, b) => {
            const amountA = new Date(a.getElementsByTagName('td')[4].textContent);
            const amountB = new Date(b.getElementsByTagName('td')[4].textContent);
            return sortFactor * (amountA - amountB);
        });

        const tbody = document.getElementById("transaction-table-body");
        rows.forEach(row => tbody.appendChild(row));
    }

    //handling clear filter
    function clearFilter() {
        const rows = document.getElementById("transaction-table-body").getElementsByTagName('tr');

        for (let i = 0; i < rows.length; i++) {
            rows[i].style.display = '';
        }
    }

    //handling clear search
    function clearSearch() {
        document.getElementById("search-bar").value = ''; // Clear the search input
        clearFilter(); // Also clear any previous filter
    }

    // ---------------- Transactions page for USER only --------------------------------
    //handle transaction search by accountNo in user
    function handleUserSearch() {
        const searchTerm = document.getElementById("user-search-bar").value.trim().toLowerCase();
        const rows = document.getElementById("transactionTableBody").getElementsByTagName('tr');

        for (let i = 0; i < rows.length; i++) {
            const accountNo = rows[i].getElementsByTagName('td')[2].textContent.trim().toLowerCase();
            if (accountNo.localeCompare(searchTerm) === 0) {
                rows[i].style.display = '';
            } else {
                rows[i].style.display = 'none';
            }
        }
}

    //handle transaction search by date range
    function handleUserDateSearch() {
        const startDate = new Date(document.getElementById("user-start-date-bar").value + ", 00:00:00");
        //the .value is important to get the value of the date inputted
        const endDate = new Date(document.getElementById("user-end-date-bar").value + ", 00:00:00");
        console.log(startDate + " " +endDate);
        const rows = document.getElementById("transactionTableBody").getElementsByTagName('tr');


        for (let i = 0; i < rows.length; i++) {
            const tempDate = new Date(rows[i].getElementsByTagName('td')[4].textContent);
            if ((tempDate >= startDate) && (tempDate <= endDate)) {
                // do >= <= instead of only === bc we have DateTime instead of just Date
                rows[i].style.display = '';
            } else {
                rows[i].style.display = 'none';
            }
        }

}

//handling filter by trasnaction type
function handleUserFilter(transactionType) {
    const rows = document.getElementById("transactionTableBody").getElementsByTagName('tr');

    for (let i = 0; i < rows.length; i++) {
        const type = rows[i].getElementsByTagName('td')[1].textContent.trim(); // Assuming transaction type is in the second column
        if (type === transactionType) {
            rows[i].style.display = '';
        } else {
            rows[i].style.display = 'none';
        }
    }
}

//handling sort ascending / descending for amount
function handleUserSort(ascending) {
    const rows = Array.from(document.getElementById("transactionTableBody").getElementsByTagName('tr'));
    const sortFactor = ascending ? 1 : -1;

    rows.sort((a, b) => {
        const amountA = parseFloat(a.getElementsByTagName('td')[3].textContent);
        const amountB = parseFloat(b.getElementsByTagName('td')[3].textContent);
        return sortFactor * (amountA - amountB);
    });

    const tbody = document.getElementById("transactionTableBody");
    rows.forEach(row => tbody.appendChild(row));
}

//handling date sort ascending / descending
function handleUserDateSort(ascending) {
    const rows = Array.from(document.getElementById("transactionTableBody").getElementsByTagName('tr'));
    const sortFactor = ascending ? 1 : -1;

    rows.sort((a, b) => {
        const amountA = new Date(a.getElementsByTagName('td')[4].textContent);
        const amountB = new Date(b.getElementsByTagName('td')[4].textContent);
        return sortFactor * (amountA - amountB);
    });

    const tbody = document.getElementById("transactionTableBody");
    rows.forEach(row => tbody.appendChild(row));
}

//handling clear filter
function clearUserFilter() {
    const rows = document.getElementById("transactionTableBody").getElementsByTagName('tr');

    for (let i = 0; i < rows.length; i++) {
        rows[i].style.display = '';
    }
}

//handling clear search
function clearUserSearch() {
    document.getElementById("user-search-bar").value = ''; // Clear the search input
    clearUserFilter(); // Also clear any previous filter
}
