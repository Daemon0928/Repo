<?php 
include_once "includes/db_inc.php";

$username = mysqli_real_escape_string($connection, $_POST['username']);
$userlevel = mysqli_real_escape_string($connection, $_POST['userlevel']);
$heroname = mysqli_real_escape_string($connection, $_POST['heroname']);
$herolevel = mysqli_real_escape_string($connection, $_POST['herolevel']);
$cardcollection = mysqli_real_escape_string($connection, $_POST['cardcollection']);
$deck = mysqli_real_escape_string($connection, $_POST['deck']);

$sql0 = "SELECT id FROM users WHERE username = '" . $username . "'";

$query0 = mysqli_query($connection, $sql0) or die("Error #0: User id query failed");

$userIdResult = mysqli_fetch_assoc($query0);

$sql = "UPDATE users SET level = " . $userlevel . " WHERE username = '" . $username . "'";

$query = mysqli_query($connection, $sql) or die("Error #1: User update query failed");

$sql1 = "UPDATE userheros SET level = " . $herolevel . ", name = '" . $heroname . "' WHERE userid = " . $userIdResult['id'];

$query1 = mysqli_query($connection, $sql1) or die("Error #2: Hero update query failed");

$sql2 = "UPDATE cards SET cardcollection = '" . $cardcollection . "', deck = '" . $deck . "' WHERE userid = " . $userIdResult['id'];

$query2 = mysqli_query($connection, $sql2) or die("Error #3 Cards update query failed");

echo "0";
?>