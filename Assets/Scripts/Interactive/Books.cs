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
        new BookData("APPLEPIE MASTER", "レシピ"),
        new BookData("PIZZA COLLECTION", "レシピ"),
        new BookData("CHEESE RICEPE", "レシピ"),
        new BookData("NOVICE COOKBOOK", "レシピ"),
        new BookData("CHICKEN BREAST RICEPES", "レシピ"),
        new BookData("TURKEY FANTASY", "レシピ"),
        new BookData("ALL DAY POTATO", "レシピ"),

        new BookData("GANGSTER WITHOUT A HOME", "ファンタジー"),
        new BookData("BOYS OF FORTUNE", "ファンタジー"),
        new BookData("GIANTS AND LORDS", "ファンタジー"),
        new BookData("ENEMIES AND AGENTS", "ファンタジー"),
        new BookData("EDGE OF THE NIGHT", "ファンタジー"),
        new BookData("CASTLE WITH GOLD", "ファンタジー"),
        new BookData("ALTERING THE CHAMPIONS", "ファンタジー"),
        new BookData("BLEEDING IN THE OCEAN", "ファンタジー"),

        new BookData("HOW TO DRAW #1", "アート"),
        new BookData("HOW TO DRAW #2", "アート"),
        new BookData("HOW TO DRAW #3", "アート"),
        new BookData("HOKUSAI ART BOOK", "アート"),
        new BookData("ABSTRACT ART WORLD", "アート"),
        new BookData("ANIMAL MOVEMENT", "アート"),
        new BookData("FANTASY COLLECTION", "アート"),
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