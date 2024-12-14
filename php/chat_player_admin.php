<?php
$hostname = 'localhost';
$username = 'root';
$password = '';
$database = 'myDB';
 
try 
{
	$dbh = new PDO('mysql:host='. $hostname .';dbname='. $database, $username, $password);
} 
catch(PDOException $e) 
{
	echo '<h1>An error has occurred.</h1><pre>', $e->getMessage()
            ,'</pre>';
}
//запрос с изменением статуса участника чата на [администратор]
//$sth = $dbh->query('SELECT * FROM myDB.list_chats where');
$pr_st='admin';
$str=$_GET['php_player'];
$str1=$_GET['php_chat'];

	$sth = $dbh->prepare('update myDB.list_chats set status=:pr_status where (myDB.list_chats.player=:pr_player and myDB.list_chats.chat=:pr_chat)');
	$sth->bindParam(':pr_player',$str,PDO::PARAM_STR);
	$sth->bindParam(':pr_chat',$str1,PDO::PARAM_STR);
	$sth->bindParam(':pr_status',$pr_st,PDO::PARAM_STR);
	$sth->execute();

	$pr_st='member';
	$str=$_GET['php_admin'];
	$sth = $dbh->prepare('update myDB.list_chats set status=:pr_status where (myDB.list_chats.player=:pr_admin and myDB.list_chats.chat=:pr_chat)');
	$sth->bindParam(':pr_admin',$str,PDO::PARAM_STR);
	$sth->bindParam(':pr_chat',$str1,PDO::PARAM_STR);
	$sth->bindParam(':pr_status',$pr_st,PDO::PARAM_STR);
	$sth->execute();

?>