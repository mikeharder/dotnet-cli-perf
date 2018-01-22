package ClassLib023;

public class Class001 {
    public static String property() {
        return "ClassLib023" + " " + ClassLib008.Class001.property() + " " + ClassLib012.Class001.property();
    }
}
