using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Books : MonoBehaviour
{
    [SerializeField] public List<GameObject> Prefabs;

    [System.Serializable]
    public struct BookData
    {
        public string title;
        public string category;

        public BookData(string t, string c)
        {
            title = t;
            category = c;
        }
    }

    //[SerializeField]
    public static BookData[] bookList =
    {
        new BookData("APPLEPIE MASTER", "���V�s"),
        new BookData("PIZZA COLLECTION", "���V�s"),
        new BookData("CHEESE RICEPE", "���V�s"),
        new BookData("NOVICE COOKBOOK", "���V�s"),
        new BookData("CHICKEN BREAST RICEPES", "���V�s"),
        new BookData("TURKEY FANTASY", "���V�s"),
        new BookData("ALL DAY POTATO", "���V�s"),

        new BookData("GANGSTER WITHOUT A HOME", "�t�@���^�W�["),
        new BookData("BOYS OF FORTUNE", "�t�@���^�W�["),
        new BookData("GIANTS AND LORDS", "�t�@���^�W�["),
        new BookData("ENEMIES AND AGENTS", "�t�@���^�W�["),
        new BookData("EDGE OF THE NIGHT", "�t�@���^�W�["),
        new BookData("CASTLE WITH GOLD", "�t�@���^�W�["),
        new BookData("ALTERING THE CHAMPIONS", "�t�@���^�W�["),
        new BookData("BLEEDING IN THE OCEAN", "�t�@���^�W�["),

        new BookData("HOW TO DRAW #1", "�A�[�g"),
        new BookData("HOW TO DRAW #2", "�A�[�g"),
        new BookData("HOW TO DRAW #3", "�A�[�g"),
        new BookData("HOKUSAI ART BOOK", "�A�[�g"),
        new BookData("ABSTRACT ART WORLD", "�A�[�g"),
        new BookData("ANIMAL MOVEMENT", "�A�[�g"),
        new BookData("FANTASY COLLECTION", "�A�[�g"),
    };

    // Start is called before the first frame update
    private void Start()
    {
        for (int i = 0; i < bookList.Length; i++)
        {
            BookData temp;
            int rnd = Random.Range(0, bookList.Length);
            temp = bookList[rnd];
            bookList[rnd] = bookList[i];
            bookList[i] = temp;
        }

        // Test

        Bookshelf bookshelf = GetComponentInParent<Bookshelf>();
        while (bookList.Length > 0)
        {
            GameObject book = popRandomBook();
            book.transform.position = transform.position;
            if (bookshelf && Random.Range(0, 2) == 0)
            {
                bookshelf.addBook(book);
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public GameObject popRandomBook()
    {
        if (bookList.Length < 1) return null;

        GameObject book = Instantiate(Prefabs[Random.Range(0, Prefabs.Count - 1)]);
        BookData bd = bookList[bookList.Length - 1];
        book.GetComponent<BookObject>().setData(bd.title, bd.category);

        System.Array.Resize(ref bookList, bookList.Length - 1);

        return book;
    }
}