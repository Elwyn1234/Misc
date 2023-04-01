<?php
function connect() {
  $host = "localhost";
  $username = "sfrancisco";
  $password = "P@ssw0rd12";
  $dbname = "sfrancisco_agile";

  $conn = new mysqli($host, $username, $password, $dbname);

  if ($conn->connect_error)
    throw new Exception("Connection failed: " . $conn->connect_error);

  return $conn;
}
?>
