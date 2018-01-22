package ClassLib028;

public class Class001 {
    public static String property() {
        return "ClassLib028" + " " + ClassLib005.Class001.property() + " " + ClassLib012.Class001.property();
    }
}
