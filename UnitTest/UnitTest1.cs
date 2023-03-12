namespace UnitTest;

public class UnitTest1
{
    private readonly Matrice _matrice;
    private readonly MatriceComparer _comparer;

    public UnitTest1()
    {
        _matrice = new Matrice(new List<List<double>>() {
            new List<double>() {1,2,3},
            new List<double>() {4,5,6},
            new List<double>() {7,8,9}
        });

        _comparer = new MatriceComparer();
    }

    [Theory]
    [MemberData(nameof(DataPermutationLigne))]
    public void TestPermuationLignes(Matrice matrice, int ligne1, int ligne2)
    {
        _matrice?.PermutationLigne(ligne1, ligne2);
        Assert.Equal(matrice, _matrice, _comparer);
    }

    public static IEnumerable<Object[]> DataPermutationLigne()
    {
        yield return new Object[] 
        { 
            new Matrice(new List<List<double>>
            {
                new List<double> {4,5,6},
                new List<double> {1,2,3},
                new List<double> {7,8,9}
            }), 0, 1
        };
        yield return new Object[] 
        {
            new Matrice(new List<List<double>>
            {
                new List<double> {7,8,9},
                new List<double> {4,5,6},
                new List<double> {1,2,3}
            }), 0, 2
        };
    }

    [Theory]
    [MemberData(nameof(DataPermutationColonne))]
    public void TestPermutationColonnes(Matrice matrice, int colonne1, int colonne2)
    {
        _matrice.PermutationColonne(colonne1, colonne2);
        Assert.Equal(matrice, _matrice, _comparer);
    }

    public static IEnumerable<Object[]> DataPermutationColonne()
    {
        yield return new Object[] 
        {
            new Matrice(new List<List<double>>() 
            {
                new List<double> {2,1,3},
                new List<double> {5,4,6},
                new List<double> {8,7,9}
            }), 0, 1
        };
    }

    [Theory]
    [MemberData(nameof(DataMultiplicationColonne))]
    public void TestMultiplicationColonne(Matrice matrice, int colonne, double reel)
    {
        _matrice.MultiplicationColonne(colonne, reel);
        Assert.Equal(matrice, _matrice, _comparer);
    }

    public static IEnumerable<Object[]> DataMultiplicationColonne()
    {
        yield return new Object[] 
        {
            new Matrice(new List<List<double>>
            {
                new List<double> {2,2,3},
                new List<double> {8,5,6},
                new List<double> {14,8,9}
            }), 0, 2
        };
    }

    [Theory]
    [MemberData(nameof(DataDivisionColonne))]
    public void TestDivisionColonne(Matrice matrice, int colonne, double reel)
    {
        _matrice.DivisionColonne(colonne, reel);
        Assert.Equal(matrice, _matrice, _comparer);
    }

    public static IEnumerable<Object[]> DataDivisionColonne()
    {
        yield return new Object[] 
        {
            new Matrice(new List<List<double>>
            {
                new List<double> {0.5,2,3},
                new List<double> {2,5,6},
                new List<double> {3.5,8,9}
            }), 0, 2
        };
    }

    [Theory]
    [MemberData(nameof(DataSoustractionMatricielle))]
    public void SoustractionMatricielle(Matrice expected, Matrice matriceSoustraction)
    {
        _matrice.SoustractionMatricielle(matriceSoustraction);
        Assert.Equal(expected, _matrice, _comparer);
    }

    public static IEnumerable<Object[]> DataSoustractionMatricielle()
    {
        yield return new Object[] 
        {
            new Matrice(new List<List<double>>
            {
                new List<double> {0,0,0},
                new List<double> {0,0,0},
                new List<double> {0,0,0}
            }),
            new Matrice(3, 3, 1,2,3,4,5,6,7,8,9)
        };
        yield return new Object[]
        {
            new Matrice(new List<List<double>>
            {
                new List<double> {0,1,2},
                new List<double> {3,4,5},
                new List<double> {6,7,8}
            }),
            new Matrice(3,3,1)
        };
    }

    [Fact]
    public void TestNewMatrixOutOfBound()
    {
        Assert.Throws<Exception>(() => new Matrice(-1, -1, 2));
    }
}