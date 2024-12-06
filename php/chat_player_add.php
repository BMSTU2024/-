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
//запрос с получением игрока, который должен находиться в конкретном чате. Если его там нет, то добавляет со статусом [приглашение], иначе меняет его статус на [приглашение]
//$sth = $dbh->query('SELECT * FROM myDB.list_chats where');
$pr_st='waiting';
$str=$_GET['php_player'];
$str1=$_GET['php_chat'];
$sth = $dbh->prepare('SELECT * FROM myDB.list_chats WHERE (list_chats.player=:pr_player and myDB.list_chats.chat=:pr_chat) ');
$sth->bindParam(':pr_player',$str,PDO::PARAM_STR);
$sth->bindParam(':pr_chat',$str1,PDO::PARAM_STR);
$sth->execute();
$sth->setFetchMode(PDO::FETCH_ASSOC);
 
$result = $sth->fetchAll(); 
if (count($result) > 0) 
{
	$sth = $dbh->prepare('update myDB.list_chats set (status=:pr_status) where (list_chats.player!=:pr_player and myDB.list_chats.chat=:pr_chat)');
	$sth->bindParam(':pr_player',$str,PDO::PARAM_STR);
	$sth->bindParam(':pr_chat',$str1,PDO::PARAM_STR);
	$sth->bindParam(':pr_status',$pr_st,PDO::PARAM_STR);
	$sth->execute();
}
else{
	$sth = $dbh->prepare('insert into myDB.list_chats (player,chat,status) values (:pr_player,:pr_chat,:pr_status) ');
	$sth->bindParam(':pr_player',$str,PDO::PARAM_STR);
	$sth->bindParam(':pr_chat',$str1,PDO::PARAM_STR);
	$sth->bindParam(':pr_status',$pr_st,PDO::PARAM_STR);
	$sth->execute();
}
?>