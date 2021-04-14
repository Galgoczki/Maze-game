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
        tortenet = new  string[]{"A szöveg felgyorsitásához nyomd meg a <color=green>bal</color> egérgombot és <color=red>jobb</color> egérgombot megnyomásával meg vissza tudsz ugrani a fö oldalra",
                                "",
                                "A görgővel tudsz haladni elörébb és hátrább, ha <color=green>felfele</color> görgetsz akkor az előző szövegre megy, ha <color=red>lefele</color> görgetsz akkor a következöre szövegrészletre megy megy",//wait merre van a felfele
                                "",
                                "",
                                "",
                                "Történetünk egy varázs világban játszódik, melynek neve <b>Golgernia</b>, ahol a mágia elég hétköznapi",//btw a nevemből képeztem a Galgóczkiból a G_lg_ és a Norbertből az oer plusz nia
                                "legalábbis az átlag mágia mint mindenhol, itt is megtalálható az elit réteg ",
                                "ahova vagy beleszület, vagy nagy nehezen tehetséggel és szerencsével ",
                                "beküzdöd magad, és kivívod a többi <b>\"nemes\"</b> becsületét és tiszteletét",
                                "de ebben a világban nem a nemesek a legnagyobb hatalommal birók. ",
                                "Néha a kifejezetten bátor és tehetséges kalandorok sokkal erösebbek ",
                                "lesznek mint az az átlag élölény képes felfogni, belölük lesznek a <b>\"Hősök\"</b>,",
                                "<b>\"Diktátorok\"</b>,<b>\"Démon úrak\"</b> vagy akár <b>\"Istenek\"</b>",
                                "Ebben a világban élt egy Elf varázslólány, akit csak a kallandózó nevén ismertek ami 'Kitty Sharptooth'.",
                                "Kitty a hires <i>High Forest</i>-ből származott.",
                                "Nem volt leszármazotja egy nemesvérü elf családnak sem, ennek ellenére",
                                "az átlagnál erősebb volt benne a mágia és szorgalmasan is tanult,",
                                "de az Elf nemesség nem nézte jo szemmel az átlag por nép felemelkedését,",
                                " ezzét diszkriminálták Kitty-t és számüzték a hazájából.",
                                "Kitty Délre indult indult hazájából mely a világtérkép észak nyugaton feküdt",
                                "és ezen a területen feküdt Ennek a világnak egyik legnagyobb városa mely",
                                "nemzetségek terén elég elfogadó volt és Kitty ezt a város vette célba.",
                                "Kitty negy évet költöt ezzel az utazással mig a volt otthonából eljutott a ",
                                "célvárosba és utazása során egy kicsit tovább bövitette varázs könyvét mégha ",
                                "csak egy kicsit utazása során találkozott pár magányos varázsloval és egy démonnal a",
                                "kinek a segitségével megkötötte az élete első szerzödését egy <b>Tressym</b>-val és ",
                                "megszerezte az elsö familiárisát egy olyan szörnyet mely két láb magas macska szerü ",
                                "lény melynek szár van a hátán fekete szöre van és arany sárga szeme.",
                                "Miután Kitty elérte a város nem sokkal megérkezése során a fagyos, zord téli hidegben",
                                "a város egyik leghiresebb fogadojában megtalálta a leendő beli csapatát.",
                                "Kitty csapatában volt egy ember Borisz a fiatal, kezdő orosz akcentusu bajkeverő,",//inside jok
                                "egy közép korabeli paladin félork Gorge és egy akkor érkezet törp aki alig látszott ",
                                "ki a testét fedő hólepel alol, válát egy nagy csatabár feküdt és hátán a hó lepel ",
                                "alatt egy nagy csatapajzs volt legfobb ismertető jegye a fegyverzete mellett,",
                                "hogy melkasáig leérő vőrős szakállal rendelkezett és raszta hajjal és úgy hivták Giltúr",
                                "A csapat hosszu éveket töltött együtt mely soránsárkányokat győztek le,",
                                "Démon úrakat szoritottak vissza és Kazamatákat tisztitottak meg",
                                "háromszáz évvel a Csapat találkozása után mikor mind a leghiresebb kalandorok",
                                "lettek és Kittyin kivül aki még csak életének két harmadánál tart, ",
                                "mindanyian eltávoztak és Gilturból egy alsóbb rangu isten lett akinek emléke ",
                                "100 évente az egész kontinenst megrázo vihar okoz melyben az vizhangzik,",
                                "hogy GLORIOUS BATTLE a többi társának már csak az emléke és a Világ szobor",
                                "emlékeztet amit miután az első démon úrat gyözték le és a",
                                "Legmagasabb hegyen állitották dicsöségükért.",
                                "Kittyötven év után úgy döntött, hogy létre hozza világ leghatalmasabb Kazamtáját",
                                "melynek közepén egy régi modi labirintust helyez el és minden héten a világ",
                                "Kitty ki mostanra már csak a leghatalmasabb varázsloként ismertek ki egy varázslattal",
                                "démon úrakat állit meg és világokat hoz létre arra használja majd eme ",
                                "világnak legnagyobb erejő varázslatát, hogy egy értelmes",
                                "élőlényt véletlenszerüen ideiglenesen a Kazamatájába teleportáljon és elzárja",
                                "erre az időre az alany összes varázserejét, hogy azon tudjon szorakozni",
                                "ahogy probál kijutni a játékos, de mivel Kitty egy nagyon jó szivű elf",
                                "ezért amikor az alany eléri a Kazamata végét vagy megsérűlne akkor",
                                "vissza kerűl oda ahol volt épségben és 20 arannyal a zsebében",
                                "A játékunk ennek a Kazamatának a Labirintusában fog játszodni",
                                "ahol mi vagyunk a szerencsés(vagy szerencsétlen) lelkek akik",
                                "bekerűlteka a Kazamatába és probálnak kijutni. Sok szerencsét ;3",
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
