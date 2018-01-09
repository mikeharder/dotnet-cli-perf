package ClassLib033;

public class Class001 {
    public static String property() {
        return "ClassLib033" + " " + ClassLib004.Class001.property() + " " + ClassLib022.Class001.property();
    }
}
