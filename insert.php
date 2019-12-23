<?
	$db_host = "127.0.0.1";
	$db_user = "root";
	$db_password="apmsetup";
	$db_name="test";

	$conn = mysql_connect("$db_host","$db_user", "$db_password");
	mysql_select_db("$db_name", $conn);

	$score = $_POST["Score"];

	mysql_query("INSERT INTO mole(score) VALUES('$score')");
	mysql_close();

?>