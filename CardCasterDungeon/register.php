<?php 
include_once "includes/db_inc.php";

if (mysqli_connect_errno()) {
	echo "Error #1"; //connection failed
	exit();
}

$username = mysqli_real_escape_string($connection, $_POST['username']);
$password = mysqli_real_escape_string($connection, $_POST['password']);
$email = mysqli_real_escape_string($connection, $_POST['email']);

//check if username exists
$sql = "SELECT username FROM users WHERE username = '" . $username . "'";

$query = mysqli_query($connection, $sql) or die("2"); //username checking query failed

if (mysqli_num_rows($query) > 0) {
	echo "Error #3: Username already exists";
	exit();
}

//check if email is registered
$sql1 = "SELECT email FROM users WHERE email = '" . $email . "'";

$query1 = mysqli_query($connection, $sql1) or die("Error #4"); //email checking query failed

if (mysqli_num_rows($query1) > 0) {
	echo "Error #4: E-mail already registered";
	exit();
}

$hashedPassword = password_hash($password, PASSWORD_DEFAULT);

$sqlInsert = "INSERT INTO users (username, email, password) VALUES (?,?,?)";

$stmt = mysqli_stmt_init($connection);

if (!mysqli_stmt_prepare($stmt, $sqlInsert)) {
	echo "Error #5";
	exit();
}
mysqli_stmt_bind_param($stmt, "sss", $username, $email, $hashedPassword);
mysqli_stmt_execute($stmt);

$sqlGetUserdata = "SELECT * FROM users WHERE username = '" . $username . "'";

$queryGetUserdata = mysqli_query($connection, $sqlGetUserdata) or die("Error #6");

$userData = mysqli_fetch_assoc($queryGetUserdata);

$sqlDefaultHero = "INSERT INTO userheros (userid, herotypeid, name, level) VALUES(" . $userData['id'] . ",1,'Diana',1)";

$queryDefaultHero = mysqli_query($connection, $sqlDefaultHero) or die("Error #7 Could not create user hero");

$sqlCreateStats = "INSERT INTO playerstatistics (userid) VALUES(" . $userData['id'] . ")";

$queryCreateStats = mysqli_query($connection, $sqlCreateStats) or die("Error #8: Could not create player statistics");

$sqlCreateCards = "INSERT INTO cards (userid, cardcollection, deck) VALUES(" . $userData['id'] . ", '1x1.2x1.3x1.4x1.5x1.6x1.7x1.8x1.9x1.10x1.11x1.12x1.13x1.14x1.15x1.16x1', '1x1.2x1.3x1.4x1.5x1.6x1.7x1.8x1.9x1.10x1.11x1.12x1.13x1.14x1.15x1.16x1')";

$queryCreateCards = mysqli_query($connection, $sqlCreateCards) or die("Error #9: Could not create collection data");

$sql2 = "SELECT enemydefeatedcount, currency FROM playerstatistics WHERE userid = " . $userData['id'];

$query2 = mysqli_query($connection, $sql2) or die("Error #10: statistics query failed");

$userStats = mysqli_fetch_assoc($query2);

echo "0;Mage;Diana;1;" . $userData['id'] . ";" . $userStats['enemydefeatedcount'] . ";" . $userStats['currency'] . ";1x1.2x1.3x1.4x1.5x1.6x1.7x1.8x1.9x1.10x1.11x1.12x1.13x1.14x1.15x1.16x1;1x1.2x1.3x1.4x1.5x1.6x1.7x1.8x1.9x1.10x1.11x1.12x1.13x1.14x1.15x1.16x1";
exit();
?>