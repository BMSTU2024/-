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
 //запрос с получением всех чатов(в которых присутсвует как участник или глава) 
 //, в которых состоит конкретный игрок и название чата начинается на определённую строку
$sth = $dbh->prepare("SELECT * FROM myDB.list_chats where myDB.list_chats.player=:pr_login 
and myDB.list_chats.status!=:pr_status1 and myDB.list_chats.status!=:pr_status2 and myDB.list_chats.chat like :pr_name");
$sth->bindParam(':pr_login',$_GET['php_login'],PDO::PARAM_STR);

$pr_st1='waiting';
$pr_st2='deleted';

$sth->bindParam(':pr_status1',$pr_st1,PDO::PARAM_STR);
$sth->bindParam(':pr_status2',$pr_st2,PDO::PARAM_STR);

$strr=$_GET['php_name'].'%';
$sth->bindParam(':pr_name',$strr,PDO::PARAM_STR);
$sth->execute();
$sth->setFetchMode(PDO::FETCH_ASSOC);
$result = $sth->fetchAll(); 
if (count($result) > 0) 
{
	foreach($result as $r) 
	{
		echo $r['chat'],"_";
		echo $r['player'],"_";
		echo $r['id'],"_";
		echo $r['status'],"_";
	}
}
?>