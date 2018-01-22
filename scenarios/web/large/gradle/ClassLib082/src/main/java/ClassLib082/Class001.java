package ClassLib082;

public class Class001 {
    public static String property() {
        return "ClassLib082" + " " + ClassLib001.Class001.property() + " " + ClassLib071.Class001.property();
    }
}
