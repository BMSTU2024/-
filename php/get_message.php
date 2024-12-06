<?php
$hostname = 'localhost';
$username = 'root';
$password = '';
$database = 'myDB';
 
try 
{
	$dbh = new PDO('mysql:host='. $hostname .';dbname='. $database.';charset=UTF8', $username, $password);
} 
catch(PDOException $e) 
{
	echo '<h1>An error has occurred.</h1><pre>', $e->getMessage()
            ,'</pre>';
}
//запрос с получением всех сообщений(либо с 0, либо с определённой позиции и до конца) конкретного игрока в конкретном чате .Сортирует по дате сообщения
$par_col=(int)$_GET['php_col'];
$sth=null;
if($par_col==0){
	$sth = $dbh->prepare(
	'select * FROM myDB.message inner join myDB.list_chats on (sender = id) where ( chat =
(select ch2.chat from myDB.list_chats as ch2 where (ch2.id=:pr_id) )
)
order by data'

	);
}
else{
	$sth = $dbh->prepare(
	'select * FROM myDB.message inner join myDB.list_chats on (sender = id) where ( chat =
(select ch2.chat from myDB.list_chats as ch2 where (ch2.id=:pr_id) )
)
order by data limit :pr_col,:pr_col_lim'

	);
	$par_col_lim=$par_col+100000;
	$sth->bindParam(':pr_col',$par_col,PDO::PARAM_INT);
	$sth->bindParam(':pr_col_lim',$par_col_lim,PDO::PARAM_INT);
}

$sth->bindParam(':pr_id',$_GET['php_id'],PDO::PARAM_INT);
$dbh->query('SET NAMES UTF8');
$sth->execute();
$sth->setFetchMode(PDO::FETCH_ASSOC);
$result = $sth->fetchAll(); 
if (count($result) > 0) 
{
	foreach($result as $r) 
	{
		echo $r['id_message'],"_";
		echo $r['player'],"_";
		echo $r['sender'],"_";
		echo $r['data'], "_";
		echo $r['text'], "_";

	}
}
?>