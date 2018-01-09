package ClassLib046;

public class Class001 {
    public static String property() {
        return "ClassLib046" + " " + ClassLib005.Class001.property() + " " + ClassLib017.Class001.property() + " " + ClassLib031.Class001.property();
    }
}
