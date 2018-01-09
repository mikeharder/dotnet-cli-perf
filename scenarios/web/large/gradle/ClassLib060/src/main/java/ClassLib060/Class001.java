package ClassLib060;

public class Class001 {
    public static String property() {
        return "ClassLib060" + " " + ClassLib050.Class001.property() + " " + ClassLib014.Class001.property();
    }
}
