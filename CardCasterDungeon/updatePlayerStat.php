<?php 
include_once "includes/db_inc.php";

$userID = mysqli_real_escape_string($connection, $_POST['userID']);
$enemyDefeated = mysqli_real_escape_string($connection, $_POST['enemyDefeated']);
$currency = mysqli_real_escape_string($connection, $_POST['currency']);

$sql = "UPDATE playerstatistics SET enemydefeatedcount = " . $enemyDefeated . ", currency = " . $currency . " WHERE userid = " . $userID;
$query = mysqli_query($connection, $sql) or die("Error #2 update query failed");

echo "0";
 ?>