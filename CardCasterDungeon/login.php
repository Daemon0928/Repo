<?php 
include_once "includes/db_inc.php";

if (mysqli_connect_errno()) {
	echo "Error #1"; //connection failed
	exit();
}

$username = mysqli_real_escape_string($connection, $_POST['username']);
$password = mysqli_real_escape_string($connection, $_POST['password']);

$sql = "SELECT id, username, password, level FROM users WHERE username = '" . $username . "'";

$query = mysqli_query($connection, $sql) or die("Error #2: Name check query failed");

if (mysqli_num_rows($query) != 1) {
	echo "Error #5: Username could not be found";
	exit();
}

$userData = mysqli_fetch_assoc($query);

if (!password_verify($password, $userData['password'])) {
	echo "Error #6: Incorrect password";
	exit();
}

$sql1 = "SELECT type, name, level FROM userheros INNER JOIN herotypes ON userheros.herotypeid	= herotypes.id WHERE userheros.userid = '" . $userData['id'] . "'";
$query1 = mysqli_query($connection, $sql1) or die("Error #6: could not get hero data");

$heroData = mysqli_fetch_assoc($query1);

$sql2 = "SELECT enemydefeatedcount, currency FROM playerstatistics WHERE userid = " . $userData['id'];

$query2 = mysqli_query($connection, $sql2) or die("Error #7: statistics query failed");

$userStats = mysqli_fetch_assoc($query2); 

$sqlCards = "SELECT cardcollection, deck FROM cards WHERE userid = " . $userData['id'];

$queryCards = mysqli_query($connection, $sqlCards) or die("Error #8: couldn't get collection data");

$collectionData = mysqli_fetch_assoc($queryCards);

echo "0;" . $userData['level'] . ";" . $heroData['type'] . ";" . $heroData['name'] . ";" . $heroData['level'] . ";" . $userData['id'] . ";" . $userStats['enemydefeatedcount'] . ";" . $userStats['currency'] . ";" . $collectionData['cardcollection'] . ";" . $collectionData['deck'];
?>