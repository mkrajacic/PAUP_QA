CREATE DATABASE qbox_db
	CHARACTER SET utf8mb4
	COLLATE utf8mb4_general_ci;
	
CREATE TABLE user_level(
  code VARCHAR(5) NOT NULL,
  name VARCHAR(255) NOT NULL,
  PRIMARY KEY (code)
);

 CREATE TABLE users(
	id INT AUTO_INCREMENT NOT NULL PRIMARY KEY,
	name VARCHAR(255),
	password VARCHAR(255),
	salt VARCHAR(255),
	image BLOB,
	user_code VARCHAR(5),
	FOREIGN KEY (user_code) REFERENCES user_level(code) on update cascade on delete cascade
 );

CREATE TABLE question_categories(
	id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
	category_name VARCHAR(255)
);

CREATE TABLE questions(
	id INT AUTO_INCREMENT NOT NULL PRIMARY KEY,
	user_id INT NOT NULL,
	question TEXT,
	datetime_created DATETIME,
	category_id INT NOT NULL,
  foreign key (user_id) references users(id) on update cascade on delete cascade,
  foreign key (category_id) references question_categories(id) on update cascade on delete cascade
); 

CREATE TABLE question_answers(
	id INT AUTO_INCREMENT NOT NULL PRIMARY KEY,
	user_id INT NOT NULL,
	question_id INT,
	answer TEXT,
	datetime_created DATETIME,
	is_favorite BIT NOT NULL DEFAULT 0,
  	foreign key (question_id) references questions(id) on update cascade on delete cascade,
	foreign key (user_id) references users(id) on update cascade on delete cascade
);

insert into user_level values ("AD","Administrator");
insert into user_level values ("RK","Registriran");
insert into users(name,password,salt,image,user_code) values ("admin","GEAgPfJmCGtHsET8lZCgipfuk3g=","1000:mLWVNRugI1WMZcZEsHqxuKKF8aAG/zJa","~/img/unnamed.png","AD");
insert into users(name,password,salt,user_code) values ("korisnik","WmU9kMF4QmoqNXKuR4O7dqr9DyM=","1000:hmLDP+rp1z1P9Z7poEZ9iP/fpmJYjrJl","RK");
insert into users(name,password,salt,user_code) values ("maverick","iKqSB17PPUHfAt2hUslEAF/Vlic=","1000:lcwsL49yLEoLuDk8DqwkUkBGWXsyQyN9","RK");
insert into users(name,password,salt,user_code) values ("Billingsgate","eV4mCjGcr0Ysy0iMYKI8tbiDyY0=","1000:jIUn/HLUMPMVeUUlOe0G5q11N321rBru","RK");
insert into users(name,password,salt,user_code) values ("Brigandine","DcTRjnMBGOcupu45h+mBeF22Pgc=","1000:X0wRbMLfyu1u4+z1wuY1rRcyi1Xn5EAZ","RK");
insert into users(name,password,salt,user_code) values ("Algot0Prore","577WUCJoY6SkURpVK/BJtonMsjs=","1000:O60/VX9WLPjQSPRJ08E8f131+EUbf7CO","RK");
insert into users(name,password,salt,user_code) values ("Snicket","atF2i4e7r9JEql6O3Oock1qKlpo=","1000:VlCtGQcPKeei4ryljnLRaDRBMufqW9tn","RK");
insert into users(name,password,salt,user_code) values ("vertebriform","gbm9FNZAVqOHPM5Z9emH7sAr+GY=","1000:gbV1i5IEXGMQw05zZfJO7lCfMNMDlxzZ","RK");
insert into users(name,password,salt,user_code) values ("luminous","Dhr+Pf9inkUGab4fsqWs4X1AUTk=","1000:QpmUpPb1YT1HkvnxM4XESONFUla7FbDF","RK");
insert into users(name,password,salt,user_code) values ("MpuczkoEcru","KCL7Ni6xk9oZUHiwchJ4wz6jipA=","1000:LJin2Zed6Xxtih+hIOYUXip9SyuK/bbX","RK");
insert into users(name,password,salt,user_code) values ("Phrontistery","wwUIm7wIJ37aXtNk/nhMFbK90KE=","1000:UmMhL0Plrwsg1gs/1vlXeABHnAY5fFHB","RK");
insert into users(name,password,salt,user_code) values ("Machinations","UzmnYaKMNTk/BEfue40jTDcFaps=","1000:UmMhL0Plrwsg1gs/1vlXeABHnAY5fFHB","RK");
insert into users(name,password,salt,user_code) values ("Genteel","HnaIhc2wWG677TrQSHDD2ociJNA=","1000:cJ6/pMC/fw1S3TSdQxNSk3I0UPfoWX+U","RK");
insert into users(name,password,salt,user_code) values ("Foxyrog5123","xqvBeiZ2Fw5KOSHkR5/xQ8n/hNE=","1000:Jvt6zIoebpt87Ej68Pty6q506Mh4j4G4","RK");
insert into users(name,password,salt,user_code) values ("Occiput","aIYEGoW+No5da0uU2SN6hDE6qMo=","1000:6HBSOreJXz4wo/u9hcT1pzskJ+yDiDYN","RK");
insert into users(name,password,salt,user_code) values ("TanisJunket","wetgAJ/NIpb1gWdiuH8YnGt4T/M=","1000:53+VHBFTRzAjg+Hlcfs4ayhCw+pHkH/9","RK");
insert into users(name,password,salt,user_code) values ("emilia","VDDoBpNpT2u70oxhMBLarQXx12w=","1000:bE/8P98zg8pngm25YowvP4R55EP60wvC","AD");
insert into users(name,password,salt,user_code) values ("puck0","Jy87nCVMVFUaMb7hnOp9tZxfUo8=","1000:v0V2l1G/Bu1QDbjFZeYtY1vd0kCYL0Bt","RK");

insert into users(name,password,salt,user_code) values ("crusch","TpnK89Zr2ZMvnlIScDZLZfs0Vn0=","1000:TBO3Dwuw8QGX2wP0GcmteymxJRCBHzPb","AD");
insert into users(name,password,salt,user_code) values ("felix","BtvPa8pfDwVue5AUU/28klgpbp4=","1000:rEL7CUv9FvF4zJqeAnxdhTg5RYMzY8Ot","RK");
insert into users(name,password,salt,user_code) values ("subaru","Roww4SKBU8NnsQVPJ6EXgMhPv3g=","1000:MLVm0wC70Rc5cESsnEVUmeOqyKYTfaUI","RK");
insert into users(name,password,salt,user_code) values ("isuzu","hJgt0fd84+hHvyFHP+ZuI7wdYEA=","1000:aEsyoI0izsSnR7s9FgHgbOcY/KmWFNhs","RK");
insert into users(name,password,salt,user_code) values ("TheroyHaft","Lnn52IG9gK+PkPZV0d1KNp/Ky/8=","1000:EY8XdCAjpXMoZ+BP2/0vV0ViWBdKEJ4i","RK");
insert into users(name,password,salt,user_code) values ("christina","ASwip7HC0wkgwiESw38o3hoPNDY=","1000:Vh8goaSw7Fpi6B25Y7u+JYP/CZagILBd","RK");
insert into users(name,password,salt,user_code) values ("rkubica","f671iqHXZnC2ZSExXkwkjDGeGLI=","1000:AGOOmCLPLRwHa8hW622hxuzTwuCF/eNN","RK");
insert into users(name,password,salt,user_code) values ("kurisu","G7LaNeyHUChB+lAimdDQHnOrRE4=","1000:pOObErEVYfwMbXGBYkakKWUyk6WcLebp","AD");
insert into users(name,password,salt,user_code) values ("homura","f/pRYM6jrrdjdDGbVhH7GQyYDAQ=","1000:kRwoQW4F354NfG268xdfBUfZjxd104PU","AD");
insert into users(name,password,salt,user_code) values ("kurapika","SBDCJvkWVs/xkzM6wArNBqoV2v0=","1000:AMfrJ5cHXYHjhZyNJLjptt2xrv4IMtzz","AD");
insert into users(name,password,salt,user_code) values ("yuki","J8oMYyzmp+mCZVjAebJ101vUiJ4=","1000:UbwPhSKysPvnsJHZpfMdKl2IaYAtesBN","RK");
insert into users(name,password,salt,user_code) values ("honda","kWSdJmmj8vSM3/VeOCG+09Jr2HQ=","1000:VpY7TIzUZittqDYftxxZtd5J4ky+w4Va","RK");

insert into question_categories (category_name) values ("Fakultet");
insert into question_categories (category_name) values ("Život");
insert into question_categories (category_name) values ("Formula 1");
insert into question_categories (category_name) values ("Programiranje");
insert into question_categories (category_name) values ("Anime");

insert into questions (question,datetime_created,user_id,category_id) values ("Koja je najluða stvar koju ste dosad napravili?","2019-11-14",1,2);
insert into questions (question,datetime_created,user_id,category_id) values ("Koje pitanje mrzite kad vam postavljaju?","2019-02-04",14,2);
insert into questions (question,datetime_created,user_id,category_id) values ("Kojim rijeèima biste se opisali?","2019-10-05",8,2);
insert into questions (question,datetime_created,user_id,category_id) values ("Ako ne slavite roðendane, zašto?","2019-11-14",15,2);
insert into questions (question,datetime_created,user_id,category_id) values ("Kada æe se održati izvandredni ispitni rok na MEV-u?","2020-05-10",5,1);
insert into questions (question,datetime_created,user_id,category_id) values ("Kada æe se nastaviti nastava na veleuèilištu?","2020-04-14",3,1);
insert into questions (question,datetime_created,user_id,category_id) values ("Tko održava vježbe iz PAUP-a na MEV-u?","2020-03-10",1,1);
insert into questions (question,datetime_created,user_id,category_id) values ("Koliko traje studij Raèunarstva na MEV-u?","2019-01-07",7,1);
insert into questions (question,datetime_created,user_id,category_id) values  ("Postoje li diplomski studiji na MEV-u?","2018-08-29",12,1);
insert into questions (question,datetime_created,user_id,category_id) values ("Koliko student po predmetu ima ispitnih rokova?","2019-11-14",1,1);

insert into questions (question,datetime_created,user_id,category_id) values ("Kada æe se održati prva F1 utrka ove sezone?","2020-06-14",25,3);
insert into questions (question,datetime_created,user_id,category_id) values ("Kamo prelazi Sebastian Vettel?","2020-05-30",27,3);
insert into questions (question,datetime_created,user_id,category_id) values ("Prikazuju li hrvatske televizije anime?","2019-03-04",18,5);
insert into questions (question,datetime_created,user_id,category_id) values ("Koji se programski jezik najbrže može savladati?","2017-10-23",12,4);
insert into questions (question,datetime_created,user_id,category_id) values ("Jesu li anime serije popularne u Hrvatskoj?","2020-02-10",27,5);
insert into questions (question,datetime_created,user_id,category_id) values ("Hoæe li Sebastian Vettel ostati u F1?","2020-05-31",4,3);
insert into questions (question,datetime_created,user_id,category_id) values ("Tko je posljednji svjetski prvak F1 iz Njemaèke?","2019-01-18",4,3);
insert into questions (question,datetime_created,user_id,category_id) values ("Ima li hrvatskih vozaèa u F1?","2018-03-05",8,3);
insert into questions (question,datetime_created,user_id,category_id) values ("Zašto je Python sve popularniji?","2020-06-03",15,4);
insert into questions (question,datetime_created,user_id,category_id) values ("Je li sigurno putovati?","2020-04-14",27,2);
insert into questions (question,datetime_created,user_id,category_id) values ("Zašto se završni ispiti ne održavaju online?","2020-03-05",12,1);
insert into questions (question,datetime_created,user_id,category_id) values ("Hoæe li se održati Porcijunkulovo?","2020-05-27",23,2);
insert into questions (question,datetime_created,user_id,category_id) values ("Hoæe li se održati Špancirfest?","2020-03-24",12,2);
insert into questions (question,datetime_created,user_id,category_id) values ("Je li sigurno održavati koncerte?","2020-03-28",19,2);
insert into questions (question,datetime_created,user_id,category_id) values ("Što su to ECTS bodovi?","2019-11-12",2,1);
insert into questions (question,datetime_created,user_id,category_id) values ("Hoæe li Mercedes ostati u F1?","2020-03-04",17,3);
insert into questions (question,datetime_created,user_id,category_id) values ("Kada æe se otvoriti studentske menze?","2020-04-24",8,1);
insert into questions (question,datetime_created,user_id,category_id) values ("Gdje iæi na struènu praksu?","2020-05-11",9,1);
insert into questions (question,datetime_created,user_id,category_id) values ("Koliko utrka æe se održati 2020?","2020-06-01",18,3);
insert into questions (question,datetime_created,user_id,category_id) values ("Koristi li se više C++?","2020-01-04",13,4);

insert into question_answers (user_id,question_id,answer,datetime_created) values (14,10,"Ima ih sest","2019-12-10");
insert into question_answers (user_id,question_id,answer,datetime_created,is_favorite) values (11,7,"Dino Kalamari","2020-03-11",1);
insert into question_answers (user_id,question_id,answer,datetime_created) values (3,3,"Luðak","2019-10-06");
insert into question_answers (user_id,question_id,answer,datetime_created,is_favorite) values (6,3,"Kao netko tko mrzi ovo pitanje","2019-10-07",1);


insert into question_categories (category_name) values ("Sport");  
  insert into question_categories (category_name) values ("Filmovi");     
  insert into question_categories (category_name) values ("Video igre");   
  insert into question_categories (category_name) values ("Reket za stolni tenis");   
  insert into question_categories (category_name) values ("Hrana");   


  insert into questions (question,datetime_created,user_id,category_id) values ("Za koji sport treba imati najbolju fizicku spremu?","2020-03-04",1,6);
  insert into questions (question,datetime_created,user_id,category_id) values ("Postoji li natjecanje na kojem se igraju samo sportovi s reketom?","2019-11-12",2,6);
  insert into questions (question,datetime_created,user_id,category_id) values ("Tko je najplaæeniji sportaš u 2020?","2020-05-07",3,6);
  insert into questions (question,datetime_created,user_id,category_id) values ("Što rade profesionalni sportaši nakon što se umirove?","2019-05-13",4,6);
  insert into questions (question,datetime_created,user_id,category_id) values ("Koji sport je najopasniji za život?","2019-10-12",5,6);
  insert into questions (question,datetime_created,user_id,category_id) values ("Zašto je nogomet najgledaniji sport?","2019-09-13",6,6);


  insert into questions (question,datetime_created,user_id,category_id) values ("Koji film ima najveæu zaradu u povijesti?", "2020-01-03",7,7);
  insert into questions (question,datetime_created,user_id,category_id) values ("Koji je najbolje ocijenjeni film?","2019-11-02",8,7);
  insert into questions (question,datetime_created,user_id,category_id) values ("Kolko se filmova snimi godišnje u Hollywoodu?","2019-05-10",9,7);
  insert into questions (question,datetime_created,user_id,category_id) values ("Koliko ljudi treba da se snimi film?","2019-07-03",1,7);
  insert into questions (question,datetime_created,user_id,category_id) values ("koji režiser je najbolji?","2019-11-01",2,7);
  insert into questions (question,datetime_created,user_id,category_id) values ("Ko je najbolji glumac?", "2020-02-12",3,7);
 

  insert into questions (question,datetime_created,user_id,category_id) values ("Kada æe video igre i e-sport biti na olimpijadi?", "2019-10-04",4,8);
  insert into questions (question,datetime_created,user_id,category_id) values ("koja je prva vieo igra?","2019-08-24",5,8);
  insert into questions (question,datetime_created,user_id,category_id) values ("Koja video igra je najprodavanija?","2019-08-10",6,8);
  insert into questions (question,datetime_created,user_id,category_id) values ("Koja video igra je trenutnu najpopularnija?","2019-07-03",7,8);
  insert into questions (question,datetime_created,user_id,category_id) values ("Koliko zaraðuju profesionalci?","2019-02-01",8,8);
  insert into questions (question,datetime_created,user_id,category_id) values ("Koja tvrtka radi najbolje video igre?", "2020-04-12",9,8);


  insert into questions (question,datetime_created,user_id,category_id) values ("Koja je razlika izmeðu napadackih i obrambenih reketa?", "2019-03-04",1,9);
  insert into questions (question,datetime_created,user_id,category_id) values ("Koje rekete ili gume koriste profesionalci?","2019-12-11",2,9);  
  insert into questions (question,datetime_created,user_id,category_id) values ("Koji je najbolji reket za mene?","2019-11-15",3,9);
  insert into questions (question,datetime_created,user_id,category_id) values ("Zašto su gume crvene i crne boje?","2020-02-18",4,9);
  insert into questions (question,datetime_created,user_id,category_id) values ("Koliko košta najskuplji reket na svijetu?","2019-12-06",5,9);
  insert into questions (question,datetime_created,user_id,category_id) values ("Koliki je vijek trajanja reketa?", "2019-09-12",6,9);


  insert into questions (question,datetime_created,user_id,category_id) values ("Zašto je zdrava hrana toliko bitna za kondicijsku pripremu?", "2019-10-22",7,10);
  insert into questions (question,datetime_created,user_id,category_id) values ("Koliko je zdrava keto-prehrana?","2019-12-23",8,10);
  insert into questions (question,datetime_created,user_id,category_id) values ("Zašto su ribe i morski proizvodi zdravi?","2019-02-10",9,10);
  insert into questions (question,datetime_created,user_id,category_id) values ("Koliko je opasno jesti puno šeæera?","2020-02-09",1,10);
  insert into questions (question,datetime_created,user_id,category_id) values ("Što jede profesionalni strongmen?","2019-06-01",2,10);
  insert into questions (question,datetime_created,user_id,category_id) values ("Koje jelo je najbolje?", "2019-09-15",3,10);









  insert into question_answers (user_id,question_id,answer,datetime_created) values (1,31,"Za vaterpolo se smatra da zahtjeva najveæu fizièku spremu.","2020-03-05");
  insert into question_answers (user_id,question_id,answer,datetime_created) values (4,32,"posotji, zove se Recketlon.","2019-11-13");
  insert into question_answers (user_id,question_id,answer,datetime_created) values (5,33,"Najplaæeniji sportaš za 2020 je tenisaè Roger Federer.","2020-05-08");
  insert into question_answers (user_id,question_id,answer,datetime_created) values (8,34,"Neki se opuste u biznis,neki se bace u trenerske vode.","2019-05-14");
  insert into question_answers (user_id,question_id,answer,datetime_created) values (12,35,"Boks i MMA se smatraju najopasnijim sportovima na svijetu .","2020-10-13");
  insert into question_answers (user_id,question_id,answer,datetime_created) values (14,36,"Neki od razloga su zato što godine nisu bitne i strast za igrom.","2019-09-14");


  insert into question_answers (user_id,question_id,answer,datetime_created) values (22,37,"Marvelov Avenegers end game ima najveæu zaradu u povijesti preko 2.7 milijardi dolara .","2020-01-05");
  insert into question_answers (user_id,question_id,answer,datetime_created) values (23,38,"Po IMDBu to je Shawshank redemption.","2019-11-13");
  insert into question_answers (user_id,question_id,answer,datetime_created) values (2,39,"Prosjeèno oko 2570 filmova godišnje.","2019-05-13");
  insert into question_answers (user_id,question_id,answer,datetime_created) values (25,40,"Prosjeèno oko 300 ljudi.","2020-07-14");
  insert into question_answers (user_id,question_id,answer,datetime_created) values (28,41,"Najbolji je Steven Spielberg.","2019-11-05");
  insert into question_answers (user_id,question_id,answer,datetime_created) values (11,42,"Svake godine netko drugi, ali najbolji se smatra Al Pacino.","2020-02-13");



  insert into question_answers (user_id,question_id,answer,datetime_created) values (16,43,"Pregovori traju da se e-sport uvede na olimpijadu no nezna se tocno.","2020-10-05");
  insert into question_answers (user_id,question_id,answer,datetime_created) values (17,45,"Tennis for two,1958, nešto slicno Pongu.","2019-08-26");
  insert into question_answers (user_id,question_id,answer,datetime_created) values (18,46,"Minecraft.","2019-08-11");
  insert into question_answers (user_id,question_id,answer,datetime_created) values (9,47,"Call of Duty Warzone.","2020-07-13");
  insert into question_answers (user_id,question_id,answer,datetime_created) values (19,48,"Više od 5 milijuna godišnje.","2020-02-05");
  insert into question_answers (user_id,question_id,answer,datetime_created) values (3,49,"Nintendo se smatra najboljim.","2020-05-13");




  insert into question_answers (user_id,question_id,answer,datetime_created) values (29,50,"Glavna razlika je u brzini.","2019-03-07");
  insert into question_answers (user_id,question_id,answer,datetime_created) values (27,51,"Najviše koriste Butterfly proizvode specificno Tenergy ili Dignics.","2019-12-13");
  insert into question_answers (user_id,question_id,answer,datetime_created) values (26,52,"Svaki igraè je svoja ruka pa se zato razlikuju najbolji reketi za svakoga.","2019-12-05");
  insert into question_answers (user_id,question_id,answer,datetime_created) values (10,53,"Razlike nema,pravilo postoji radi fair playa da protivnièki igraè vidi s kojom gumom se loptica odigrala.","2020-02-20");
  insert into question_answers (user_id,question_id,answer,datetime_created) values (15,54,"Najskuplji reket košta otprilike 500 eura.","2019-12-08");
  insert into question_answers (user_id,question_id,answer,datetime_created) values (13,55,"Profesionalci dobivaju nove rekete svakih 3-4 tjedana,a prosjek je 3 mjeseca ovisi od reketa do reketa.","2019-12-13");




  insert into question_answers (user_id,question_id,answer,datetime_created) values (1,56,"Zato što iz zdrave hrane se dobivaju najbolji vitamini i minerali potrebni za zdrav život i kondiciju.","2020-10-25");
  insert into question_answers (user_id,question_id,answer,datetime_created) values (14,57,"Keto ili ketogena prehrana je dijeta s niskim udjelom ugljikohidrata i dijeta s više masti.","2019-12-15");
  insert into question_answers (user_id,question_id,answer,datetime_created) values (15,58,"Zato što sadrže puno masti koje su jako zdrave za ljude.","2019-02-15");
  insert into question_answers (user_id,question_id,answer,datetime_created) values (18,59,"Puno šeæera može dovesti do sljepoæe pa èak i smrti.","2020-03-13");
  insert into question_answers (user_id,question_id,answer,datetime_created) values (26,60,"Preko 10000 kalorija na dan,puno mesa i proteina.","2019-07-05");






insert into question_categories (category_name) values ("Moda");
insert into question_categories (category_name) values ("Automobili");
insert into question_categories (category_name) values ("Slobodno vrijeme");
insert into question_categories (category_name) values ("Internet");
insert into question_categories (category_name) values ("Glazba");

insert into questions (question,datetime_created,user_id,category_id) values ("Koja je haljina za koji oblik tijela?","2020-02-04",1,11);
insert into questions (question,datetime_created,user_id,category_id) values ("Što odjenuti na prvi dan fakulteta?","2019-12-12",2,11);
insert into questions (question,datetime_created,user_id,category_id) values ("Koje boje haljine ide uz crvene štikle?","2020-05-01",2,11);
insert into questions (question,datetime_created,user_id,category_id) values ("Koja boje kose mi najbolje pristaje uz smeðe oèi?","2019-02-13",10,11);
insert into questions (question,datetime_created,user_id,category_id) values ("Što odjenuti na zakazani termin za posao?","2019-11-12",3,11);
insert into questions (question,datetime_created,user_id,category_id) values ("Kako hodati u štiklama?","2019-10-13",3,11);

insert into questions (question,datetime_created,user_id,category_id) values ("Kada je zabilježena prva automobilska nesreæa?","2020-03-04",10,12);
insert into questions (question,datetime_created,user_id,category_id) values ("Kada je postavljen prvi semafor?","2019-12-24",12,12);
insert into questions (question,datetime_created,user_id,category_id) values ("Koji je najskuplji automobil na svijetu?","2019-11-10",1,12);
insert into questions (question,datetime_created,user_id,category_id) values ("Koliko se otprilike automobila koristi u svijetu?","2020-02-03",2,12);
insert into questions (question,datetime_created,user_id,category_id) values ("Da li je moguæe pokvariti kljuè od automobila?","2019-12-01",2,12);
insert into questions (question,datetime_created,user_id,category_id) values ("Koje vrste elektriènih automobila postoje?","2019-09-12",3,12);

insert into questions (question,datetime_created,user_id,category_id) values ("Koliko puta tjedno bi trebalo vježbati, tj aktivno se baviti s neèime?","2019-12-03",8,13);
insert into questions (question,datetime_created,user_id,category_id) values ("Èemu služi mali džepiæ na hlaèama?","2020-01-12",6,13);
insert into questions (question,datetime_created,user_id,category_id) values ("Kako organizirati ormar?","2020-02-02",7,13);
insert into questions (question,datetime_created,user_id,category_id) values ("Kako odabrati pravo ronilaèko odijelo?","2019-10-23",16,13);
insert into questions (question,datetime_created,user_id,category_id) values ("Kako napraviti nevidljivu tintu?","2020-05-06",17,13);
insert into questions (question,datetime_created,user_id,category_id) values ("Da li je bolje vježbati ujutro ili naveèer?","2020-03-12",12,13);

insert into questions (question,datetime_created,user_id,category_id) values ("Što je internet?","2019-11-12",12,14);
insert into questions (question,datetime_created,user_id,category_id) values ("Što je spam?","2020-05-23",15,14);
insert into questions (question,datetime_created,user_id,category_id) values ("Èime se odreðuje brzina prijenosa podataka na interentu?","2020-05-01",17,14);
insert into questions (question,datetime_created,user_id,category_id) values ("Tko su hakeri?","2019-10-13",17,14);
insert into questions (question,datetime_created,user_id,category_id) values ("Što CMS sustavi omoguæuju?","2020-04-03",15,14);
insert into questions (question,datetime_created,user_id,category_id) values ("Koja je kratica za internetski davatelj mrežnih usluga?","2020-01-01",10,14);

insert into questions (question,datetime_created,user_id,category_id) values ("Kada je umro Avicii?","2020-04-04",3,15);
insert into questions (question,datetime_created,user_id,category_id) values ("Koliko pjesama je otpjevao Elvis Presley?","2020-02-02",13,15);
insert into questions (question,datetime_created,user_id,category_id) values ("Kako se zovu èlanice ABBA benda?","2019-12-11",14,15);
insert into questions (question,datetime_created,user_id,category_id) values ("Kojim se sportom bavio Dino Dvornik?","2020-03-03",16,15);
insert into questions (question,datetime_created,user_id,category_id) values ("Kako su se zvale gitare BB Kinga?","2020-05-06",11,15);
insert into questions (question,datetime_created,user_id,category_id) values ("Najbolja hrvatska klasièna gitaristica?","2020-04-05",10,15);



insert into question_answers (user_id,question_id,answer,datetime_created) values (1,61,"Postoje 6 vrsta oblika tijela i za svaki oblik su preporuèene neke vrste haljina.","2020-02-05");
insert into question_answers (user_id,question_id,answer,datetime_created) values (1,62,"Odjeni ono u èemu se najbolje osjeæaš.","2019-12-13");
insert into question_answers (user_id,question_id,answer,datetime_created) values (3,63,"Najbolje crna haljina.","2020-05-03");
insert into question_answers (user_id,question_id,answer,datetime_created) values (1,64,"Uz smeðe oèi pristaju sve boje oèiju","2019-02-14");
insert into question_answers (user_id,question_id,answer,datetime_created) values (1,65,"Najbolje neke sveèanije hlaèe i košulju.","2020-01-13");
insert into question_answers (user_id,question_id,answer,datetime_created) values (1,66,"Ako ne znas hodati u njima bolje ih ne nositi.","2019-10-14");

insert into question_answers (user_id,question_id,answer,datetime_created) values (2,67,"Prva automobilska nesreæa zabilježena je 1769.godine.","2020-03-05");
insert into question_answers (user_id,question_id,answer,datetime_created) values (3,68,"1914.godine","2019-12-28");
insert into question_answers (user_id,question_id,answer,datetime_created) values (4,69,"Najskuplji automobil ikad proizveden je Buggati Royal Kellner Coupe. Cijena? Samo 8,7 milijuna dolara.","2019-11-13");
insert into question_answers (user_id,question_id,answer,datetime_created) values (1,70,"1 milijarda automobila","2020-02-04");
insert into question_answers (user_id,question_id,answer,datetime_created) values (1,71,"Da, moguæe je. Npr.ako daljinski automobila pritisnete 256 puta, a da vam auto nije u blizini, kljuæ æe prestati raditi.","2019-12-02");
insert into question_answers (user_id,question_id,answer,datetime_created) values (1,72,"Postoje tri vrste vozila koja u veæoj ili manjoj mjeri rade na struju. Prvo, hibrid s dva motora, s motorom s unutrašnjim izgaranjem i elektriènim motorom, gdje se baterija puni kad vozilo smanjuje brzinu. Drugo, plug-in hibrid, gdje se baterija takoðer može puniti izravno prikljuèivanjem. Napokon, 100% elektrièni automobil, s iskljuèivo elektriènim motorom i dodatnom punjivom baterijom.","2019-09-13");

insert into question_answers (user_id,question_id,answer,datetime_created) values (3,73,"Najmanje 2 puta.","2019-12-05");
insert into question_answers (user_id,question_id,answer,datetime_created) values (3,74,"Da ste kauboj iz 19. stoljeæa istog bi vam trena bilo jasno èemu služi, ali buduæi da živite u modernim vremenima, tamo vjerojatno držite upaljaè. No, ideja je bila da tu smjestite džepni sat.","2020-01-16");
insert into question_answers (user_id,question_id,answer,datetime_created) values (2,75,"Razvrstajte odjeæu u dvije hrpe. Napravite dvije razlièite gomile: hrpu zadrži i hrpu riješi se.","2020-02-04");
insert into question_answers (user_id,question_id,answer,datetime_created) values (1,76,"Provjerite pripija li se odijelo tijesno uz vas. Smjelo bi propuštati samo minimalnu kolièinu vode.","2019-10-20");
insert into question_answers (user_id,question_id,answer,datetime_created) values (2,77,"Nevidljive poruke možete pisati svježe iscijeðenim limunovim sokom ili limunovim sokom iz boèice ili sokom iscijeðenim od naribanoga luka.","2020-05-07");
insert into question_answers (user_id,question_id,answer,datetime_created) values (1,78,"Vježbanje ujutro može poveæati razinu energije za ostatak dana.","2020-03-15");

insert into question_answers (user_id,question_id,answer,datetime_created) values (4,79,"Globalna raèunalna mreža koja se sastoji od više manjih mreža","2019-11-13");
insert into question_answers (user_id,question_id,answer,datetime_created) values (5,80,"Naziv za neželjenu poruku","2020-05-24");
insert into question_answers (user_id,question_id,answer,datetime_created) values (6,81,"bps","2020-05-05");
insert into question_answers (user_id,question_id,answer,datetime_created) values (1,82,"Osobe koje izvrsno poznaju raèunala, programe i hardver","2019-10-16");
insert into question_answers (user_id,question_id,answer,datetime_created) values (3,83,"Izradu, oblikovanje, održavanje i nadogradnju mrežnih stranica.","2020-04-06");
insert into question_answers (user_id,question_id,answer,datetime_created) values (3,84,"ISP","2020-01-03");

insert into question_answers (user_id,question_id,answer,datetime_created) values (1,85,"20.travnja 2018.godine.","2020-04-04");
insert into question_answers (user_id,question_id,answer,datetime_created) values (2,86,"600","2020-02-03");
insert into question_answers (user_id,question_id,answer,datetime_created) values (3,87,"Benny, Byorn, Agnetha, Anni-Frid","2019-12-12");
insert into question_answers (user_id,question_id,answer,datetime_created) values (5,88,"Baseball","2020-03-04");
insert into question_answers (user_id,question_id,answer,datetime_created) values (1,89,"Lucille","2020-05-07");
insert into question_answers (user_id,question_id,answer,datetime_created) values (7,90,"Ana Vidoviæ","2020-04-06");











