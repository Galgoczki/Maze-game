using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Story : MonoBehaviour
{   
    private string[] tortenet;
    private Text text;
    private int index=0;
    private int szoveghossz=5;
    private float delay=0;
    private float maxdelay=4;
    // Start is called before the first frame update
   
    void Start()
    {   //wilág Faerûn
        //elf,mage,name: Cat lady -> Cat
        tortenet = new  string[]{"A szöveg felgyorsításához nyomd meg a <color=green>bal</color> egérgombot.",
                                "A <color=red>jobb</color> egérgomb megnyomásával vissza tudsz térni a főoldalra.",
                                "A görgővel tudsz a szövegben előre, hátra haladni.",
                                "",
                                "Történetünk egy varázsvilágban játszódik, melynek neve <b>Golgernia</b>.",
                                "Golgerniában a mágia mindennapos, itt élnek a nemes varázslóurak, akik a varázsló elitbe születtek. Ebben a világban élnek az Elf-ek, akik átlagos varázsképességgel rendelkeznek. ",
                                "Az Elf-ek közül a tehetséges ifjak kerülnek a varázslóurak mellé varázslótanoncoknak.",
                                "A varázslótanoncok a varázslást a tehetségükkel és az ügyességükkel vívhatják ki és szerezhetik meg a varázslói címet. ",
                                "Ebben a világban a nemes varázslóurak bírnak a legnagyobb hatalommal.",
                                "Közülük kerülnek ki az Istenek vagy éppen a diktátor Démon urak. ",
                                "Azonban néhány alkalommal előfordult már, hogy egy bátor és tehetséges kalandor sokkal erősebb varázslóvá váltak, mint azt a nemes varázslóurak elképzelhetnék. ",
                                "Ebben a világban élt egy Elf varázslólány, akit csak kalandozó nevén ismernek. ",
                                "A lány neve Kitty Sharptooth. ",
                                "Kitty a hírhedt High Forest-ből származott és nem voltak nemesvérű családtagjai. ",
                                "Azonban varázslatai az átlagosnál erősebbek voltak és tehetségével, valamint kitartó szorgalmával kiemelkedett az Elfek világában, amit az Elf varázslók nem néztek jó szemmel. ",
                                "Ezért Kitty-t a nyugati világukból száműzték.",
                                "Kitty hazájából délre indult, mert ezen a területen található a varázslóvilág egyik legnagyobb városa, melynek varázsló népessége elfogadóbb volt. ",
                                "Kitty négy éven keresztül utazott a déli várost keresve. Utazása során bővítette varázskönyvét, mert a kalandozásai során találkozott több magányos varázslóval és egy démonnal is. ",
                                "Az új barátai segítségével megkötötte élete első szerződését egy Tressym-al, akitől megszerezte az első familiárisát. ",
                                "A familiáris egy két láb magas macska szerű szörny, melynek a szőre fekete, aranysárga a szeme és a hátán erős tollszárnya van. Ez a szörnyállat hűségesen követte gazdáját mindenhová és védelmezte őt.",
                                "Miután Kitty egy fagyos, téli estén elérte a nyugati várost, betért a város leghíresebb fogadójába, ahol ismeretséget kötött a leendő varázsló csapatával. ",
                                "Kitty csapatában volt egy kezdő bajkeverő fiatalember, aki orosz akcentussal beszélt és a neve Borisz. ",
                                " Egy középkorabeli paladin félork, akinek a neve Gorge.",
                                "És a csapat negyedik tagja egy törpe, akinek a neve Giltúr.",
                                "Giltúr olyan alacsony törpe volt, hogy alig látszott ki a frissen leesett hólepelből és a vállán egy nagy csatabárd feküdt. Fegyverzete mellett legfőbb ismertetőjegye a mellkasáig leérő vörös szakálla és a hosszú raszta haja volt. ",
                                "A csapat hosszú éveket töltött el együtt, melynek során veszélyes sárkányokat győztek le, gyilkos Démon urakat szorítottak vissza és sötét Kazamatákat tisztítottak meg. ",
                                "Háromszáz évvel a Csapat találkozása után, miután mind híres kalandorok lettek, Kitty és csapata visszavonult.",
                                "Gilturból egy alsóbb rangú Isten lett, akinek emlékére 100 évente az egész kontinenset megrázó vihar keletkezik, melyben a GLORIOUS BATTLE visszhangzik. ",
                                "Giltur emlékére a legmagasabb hegyen egy Világ szobrot emeltek, ahol az első démon urat győzte le a Csapat. ",
                                "Kitty lett a varázslóvilág leghatalmasabb varázslónője. ",
                                "Úgy döntött, hogy létrehozza a világ legrejtélyesebb Kazamatáját, melynek a közepén egy régimódi labirintust helyez el.",
                                "Kitty eme világ legnagyobb erejű varázslatával a tanítandó varázslókat ideiglenesen a Kazamatájába teleportálja, hogy próbatétel elé állítsa őket. Varázserejüket egy időre elveszi, hogy különleges képességek nélkül próbáljanak meg kijutni a labirintusból.",
                                "Kitty nagyon jó szívű Elf, ezért amikor a játékos megsérül azt épségben, 20 arannyal a zsebében visszaküldi a kezdőpontra.",
                                "A játékunk ennek a Kazamatának a Labirintusában fog játszódni, ahol mi vagyunk a szerencsés (vagy szerencsétlen) varázslótanoncok, akik bekerültek a Kazamatába és megpróbálnak kijutni onnan. ",
                                "",
                                "Sok szerencsét!!"
                                };
        text = GameObject.Find("UI_story").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {   
        //Debug.Log(delay);
        delay -= Time.deltaTime;
        if(delay<=0){
            
            delay=maxdelay;
            string vegeredmeny="";
            for(int i =0;i<szoveghossz;i++){
                if(index+i<tortenet.Length && index+i >=0)
                vegeredmeny+=tortenet[index+i]+"\n";
            }
            index++;
            text.text=vegeredmeny;
        }
        
        if(index==tortenet.Length || Input.GetMouseButtonDown(1)){
            SceneManager.LoadScene(0);
        }
        
        if (Input.GetAxis("Mouse ScrollWheel") > 0f ){//fel
            if(index-2>=0){
                index-=2;//mivel egyszer már növeltem
                delay=0;
            }else
            {
                delay=maxdelay;//hogy ne menjen el
            }
        }
        if (Input.GetMouseButtonDown(0) || Input.GetAxis("Mouse ScrollWheel") < 0f ){// kattint vagy lefele
            delay=0;
        }
    }
}
